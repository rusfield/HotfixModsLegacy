using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class CreatureDisplayInfoOption
    {
        public int Id { get; set; } = 0;
        public int ChrCustomizationOptionId { get; set; } = 0;
        public int ChrCustomizationChoiceId { get; set; } = 0;
        public int CreatureDisplayInfoExtraId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
