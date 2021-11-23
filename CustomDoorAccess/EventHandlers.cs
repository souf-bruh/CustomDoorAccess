using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MapGeneration.Distributors;
using Log = Exiled.API.Features.Log;

namespace CustomDoorAccess
{
    public class EventHandlers
    {
        private readonly CdaPlugin _plugin;
        public EventHandlers(CdaPlugin plugin) => _plugin = plugin;
        //internal List<ItemType> KeycardList = new List<ItemType> { ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardZoneManager, ItemType.KeycardResearchCoordinator, ItemType.KeycardGuard, ItemType.KeycardNTFOfficer, ItemType.KeycardNTFLieutenant, ItemType.KeycardNTFCommander, ItemType.KeycardChaosInsurgency, ItemType.KeycardContainmentEngineer, ItemType.KeycardZoneManager, ItemType.KeycardO5 };
        public void OnDoorInteract(InteractingDoorEventArgs ev)
        {
            var ply = ev.Player;
            if (!ev.Door.Base.gameObject.TryGetComponent(out DoorNametagExtension _)) return;
            var doorName = ev.Door.Base.gameObject.GetComponent<DoorNametagExtension>().GetName;
            var accessSet = _plugin.Config.AccessSet;
            if (accessSet.TryGetValue(doorName, out var valueName))
            {
                    string trimmedValue = valueName.Trim();
                    string[] itemIDs = trimmedValue.Split('&');
                    foreach (string eachValue in itemIDs)
                    {
                        int currentItem = (int) ply.Inventory.CurItem.TypeId;
                        if (int.TryParse(eachValue, out int itemId))
                        {
                            if(ply.IsBypassModeEnabled)
                            {
                                ev.IsAllowed = true;
                                return;
                            }
                            if (_plugin.Config.Scp079Bypass && ply.Role.Equals(RoleType.Scp079))
                            {
                                ev.IsAllowed = true;
                                return;
                            }
                            if (currentItem.Equals(itemId) && !currentItem.Equals(-1))
                            {
                                ev.IsAllowed = true;
                                return;
                            }
                            if (_plugin.Config.ScpAccess)
                            {
                                foreach (string scpAccessDoor in _plugin.Config.ScpAccessDoors)
                                {
                                    if (doorName.Equals(scpAccessDoor))
                                    {
                                        if (ply.ReferenceHub.characterClassManager.IsAnyScp())
                                        {
                                            ev.IsAllowed = true;
                                            return;
                                        }
                                    }
                                }
                            }
                            if (_plugin.Config.RevokeAll && !itemIDs.Contains(currentItem.ToString()))
                            {
                                ev.IsAllowed = false;
                                return;
                            }
                        }
                        else
                        {
                            Log.Error(valueName + " is not a int.");
                        }
                    }
            }
        }
        public void OnGeneratorUnlock(UnlockingGeneratorEventArgs ev)
        {
            if (!_plugin.Config.GeneratorAccess.Any()) return;
            var currItem = (int) ev.Player.Inventory.CurItem.TypeId;
            var configItemList = _plugin.Config.GeneratorAccess;

            ev.IsAllowed = configItemList.Contains(currItem);
        }
        public void OnElevatorInteraction(InteractingElevatorEventArgs ev)
        {
            var ply = ev.Player;
            var elevatorAccess = _plugin.Config.ElevatorAccess;
            string elevatorName;
            switch (ev.Lift.elevatorName)
            {
                case "GateA":
                    elevatorName = "GateA";
                    break;
                case "GateB":
                    elevatorName = "GateB";
                    break;
                case "SCP-049":
                    elevatorName = "Scp049";
                    break;
                case "ElA":
                    elevatorName = "SystemA";
                    break;
                case "ElB":
                    elevatorName = "SystemB";
                    break;
                case "ElA2":
                    elevatorName = "SystemA";
                    break;
                case "ElB2":
                    elevatorName = "SystemB";
                    break;
                default:
                    elevatorName = "Nuke";
                    break;
            }
            if(elevatorAccess.Keys.Count == 0) return;
            if (elevatorAccess.TryGetValue(elevatorName, out var elevatorValue))
            {
                string trimmedValue = elevatorValue.Trim();
                string[] itemIDs = trimmedValue.Split('&');
                foreach (var eachValue in itemIDs)
                {
                    int currentItem = (int) ply.Inventory.CurItem.TypeId;
                    if (int.TryParse(eachValue, out int itemId))
                    {
                        if(ply.IsBypassModeEnabled)
                        {
                            ev.IsAllowed = true;
                            return;
                        }
                        if (ply.ReferenceHub.characterClassManager.IsAnyScp())
                        {
                            ev.IsAllowed = true;
                            return;
                        }
                        if (currentItem.Equals(itemId) && !currentItem.Equals(-1))
                        {
                            ply.ShowHint($"{_plugin.Translation.ElevatorCanUse}");
                            ev.IsAllowed = true;
                            return;
                        }
                        if (!itemIDs.Contains(currentItem.ToString()))
                        {
                            ply.ShowHint($"{_plugin.Translation.ElevatorCantUse}");
                            ev.IsAllowed = false;
                            return;
                        }
                    }
                    else
                    {
                        Log.Error(elevatorValue + " is not a int.");
                    }
                }
            }
        }
        internal void OnLockersInteract(InteractingLockerEventArgs ev)
        {
            if (ev.Locker.StructureType == StructureType.StandardLocker)
            {
                if (!_plugin.Config.Nuke049LockersAccess.Any()) return;
                var currItem = (int)ev.Player.Inventory.CurItem.TypeId;
                var configItemList = _plugin.Config.Nuke049LockersAccess;
                ev.IsAllowed = configItemList.Contains(currItem);
                if (ev.IsAllowed == false) ev.Player.ShowHint($"{_plugin.Translation.StandardLockerCantOpen}");
            }
            if (ev.Locker.StructureType == StructureType.LargeGunLocker)
            {
                if (!_plugin.Config.LargeGunLocker.Any()) return;
                var currItem = (int)ev.Player.Inventory.CurItem.TypeId;
                var configItemList = _plugin.Config.LargeGunLocker;
                ev.IsAllowed = configItemList.Contains(currItem);
                if (ev.IsAllowed == false) ev.Player.ShowHint($"{_plugin.Translation.LargeGunLockerCantOpen}");
            }
            if (ev.Locker.StructureType == StructureType.ScpPedestal)
            {
                if (!_plugin.Config.ScpPedestalLocker.Any()) return;
                var currItem = (int)ev.Player.Inventory.CurItem.TypeId;
                var configItemList = _plugin.Config.ScpPedestalLocker;
                ev.IsAllowed = configItemList.Contains(currItem);
                if (ev.IsAllowed == false) ev.Player.ShowHint($"{_plugin.Translation.ScpPedestalCantOpen}");
            }
            if (ev.Locker.StructureType == StructureType.SmallWallCabinet)
            {
                if (!_plugin.Config.MedKitLocker.Any()) return;
                var currItem = (int)ev.Player.Inventory.CurItem.TypeId;
                var configItemList = _plugin.Config.MedKitLocker;
                ev.IsAllowed = configItemList.Contains(currItem);
                if (ev.IsAllowed == false) ev.Player.ShowHint($"{_plugin.Translation.MedKitLockerCantOpen}");
            }
        }
        internal void OnWorkstationUse(ActivatingWorkstationEventArgs ev)
        {
            if (!_plugin.Config.WorkStationAccess.Any()) return;
            var currItem = (int)ev.Player.Inventory.CurItem.TypeId;
            var configItemList = _plugin.Config.WorkStationAccess;
            ev.IsAllowed = configItemList.Contains(currItem);
            if (ev.IsAllowed == false) ev.Player.ShowHint($"{_plugin.Translation.WorkstationCantActivate}");
        }
    }
}