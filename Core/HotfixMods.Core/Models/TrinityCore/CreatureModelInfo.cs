using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureModelInfo
    {
        
        public uint DisplayID { get; set; } = 0;
        public decimal BoundingRadius { get; set; } = 0;
        public decimal CombatReach { get; set; } = 0;
        [Db2Description("The Display ID of the creature as the other gender. To utilize this for a custom creature in HotfixMods, create a new creature in the different gender and link them to each other here.$For more advanced configurations on multiple display models on the same creature, edit in TrinityCore directly.")]
        public uint DisplayId_Other_Gender { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
