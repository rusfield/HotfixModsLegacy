using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    // Helper model to load customizations, not used as hotfix
    [HotfixesSchema]
    public class ChrRaceXChrModel
    {
        public int ID { get; set; } = 0;
        public int ChrRacesID { get; set; }
        public int ChrModelID { get; set; }
        public int Sex { get; set; }
        public int AllowedTransmogSlots { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
