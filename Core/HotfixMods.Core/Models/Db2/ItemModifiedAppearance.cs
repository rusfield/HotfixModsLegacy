using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemModifiedAppearance
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ItemAppearanceModifierId { get; set; }
        public int ItemAppearanceId { get; set; }
        public int OrderIndex { get; set; }
        public byte TransmogSourceTypeEnum { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
