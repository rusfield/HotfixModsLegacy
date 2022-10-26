using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemAppearance
    {
        public int Id { get; set; }
        public int DisplayType { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public int DefaultIconFileDataId { get; set; }
        public int UiOrder { get; set; }
        public int TransmogPlayerConditionId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
