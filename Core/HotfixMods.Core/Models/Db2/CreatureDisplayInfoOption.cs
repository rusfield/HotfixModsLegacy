using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoOption
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int ChrCustomizationOptionID { get; set; } = 0;
        public int ChrCustomizationChoiceID { get; set; } = 0;
        [ParentIndexField]
        public int CreatureDisplayInfoExtraID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
