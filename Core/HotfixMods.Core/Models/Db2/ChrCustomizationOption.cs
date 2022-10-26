using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationOption
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public ushort SecondaryId { get; set; }
        public int Flags { get; set; }
        public int ChrModelId { get; set; }
        public int OrderIndex { get; set; }
        public int ChrCustomizationCategoryId { get; set; }
        public int OptionType { get; set; }
        public decimal BarberShopCostModifier { get; set; }
        public int ChrCustomizationId { get; set; }
        public int Requirement { get; set; }
        public int SecondaryOrderIndex { get; set; }
        public int AddedInPatch { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
