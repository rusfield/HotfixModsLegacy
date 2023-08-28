using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoOption
    {
        
        public int ID { get; set; } = 0;
        public int ChrCustomizationOptionID { get; set; } = 0;
        public int ChrCustomizationChoiceID { get; set; } = 0;
        
        public int CreatureDisplayInfoExtraID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
