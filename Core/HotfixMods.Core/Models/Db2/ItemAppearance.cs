using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemAppearance
    {
        public int Id { get; set; } = 0;
        public int DisplayType { get; set; } = 0;
        public int ItemDisplayInfoId { get; set; } = 0;
        public int DefaultIconFileDataId { get; set; } = 0;
        public int UiOrder { get; set; } = 0;
        public int TransmogPlayerConditionId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
