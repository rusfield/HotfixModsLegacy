using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int CustomizeIcon { get; set; }
        public int CustomizeIconSelected { get; set; }
        public int OrderIndex { get; set; }
        public int CameraZoomLevel { get; set; }
        public int Flags { get; set; }
        public int SpellShapeshiftFormId { get; set; }
        public decimal CameraDistanceOffset { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
