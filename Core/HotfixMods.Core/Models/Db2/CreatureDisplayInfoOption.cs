using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoOption
    {
        public int Id { get; set; }
        public int ChrCustomizationOptionId { get; set; }
        public int ChrCustomizationChoiceId { get; set; }
        public int CreatureDisplayInfoExtraId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
