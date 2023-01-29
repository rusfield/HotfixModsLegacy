using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // Helper model to load customizations, not used as hotfix
    [HotfixesSchema]
    public class ChrRaceXChrModel
    {
        public uint Id { get; set; } = 0;
        public int ChrRacesId { get; set; }
        public int ChrModelId { get; set; }
        public int Sex { get; set; }
        public int AllowedTransmogSlots { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
