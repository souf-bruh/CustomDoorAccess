using System.ComponentModel;
using Exiled.API.Interfaces;

namespace CustomDoorAccess
{
    public class Trasnlation : ITranslation
    {
        [Description("Text displayed after using the workstation")]
        //public string WorkstationCanActivate { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string WorkstationCantActivate { get; set; } = "<color=red>ACCESS DENIED</color>";

        [Description("Text displayed after an attempt to open locker in Nuke or SCP-049 Armory")]
        //public string StandardLockerCanOpen { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string StandardLockerCantOpen { get; set; } = "<color=red>ACCESS DENIED</color>";

        [Description("Text displayed after an attempt to open Locker with E-11-SR")]
        //public string LargeGunLockerCanOpen { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string LargeGunLockerCantOpen { get; set; } = "<color=red>ACCESS DENIED</color>";

        [Description("Text displayed after an attempt to open Scp Pedestal Locker")]
        //public string ScpPedestalCanOpen { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string ScpPedestalCantOpen { get; set; } = "<color=red>ACCESS DENIED</color>";

        [Description("Text displayed after an attempt to open Med Kit Locker")]
        //public string MedKitLockerCanOpen { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string MedKitLockerCantOpen { get; set; } = "<color=red>ACCESS DENIED</color>";

        [Description("Text displayed after using the elevator")]
        public string ElevatorCanUse { get; set; } = "<color=green>ACCESS GRANTED</color>";
        public string ElevatorCantUse { get; set; } = "<color=red>ACCESS DENIED</color>";
    }
}
