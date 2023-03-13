using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemAppearance
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public int DisplayType { get; set; } = 0;
        public int ItemDisplayInfoID { get; set; } = 0;
        public int DefaultIconFileDataID { get; set; } = 0;
        public int UiOrder { get; set; } = 0;
        public int TransmogPlayerConditionID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
