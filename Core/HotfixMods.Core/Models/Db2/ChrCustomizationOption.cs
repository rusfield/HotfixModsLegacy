using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums.Db2;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationOption
    {
        public string Name { get; set; } = "";
        public int Id { get; set; } = 0;
        public ushort SecondaryId { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public ChrModelId ChrModelId { get; set; } = ChrModelId.DEFAULT;
        public int OrderIndex { get; set; } = 0;
        public int ChrCustomizationCategoryId { get; set; } = 0;
        public int OptionType { get; set; } = 0;
        public decimal BarberShopCostModifier { get; set; } = 0;
        public int ChrCustomizationId { get; set; } = 0;
        public int Requirement { get; set; } = 0;
        public int SecondaryOrderIndex { get; set; } = 0;
        public int AddedInPatch { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
