using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemModifiedAppearance
    {
        public int Id { get; set; } = 0;
        public int ItemId { get; set; } = 0;
        public int ItemAppearanceModifierId { get; set; } = 0;
        public int ItemAppearanceId { get; set; } = 0;
        public int OrderIndex { get; set; } = 0;
        public byte TransmogSourceTypeEnum { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
