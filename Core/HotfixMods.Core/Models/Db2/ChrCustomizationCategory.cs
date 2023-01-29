using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationCategory
    {
        public uint Id { get; set; } = 0;
        public string CategoryName { get; set; } = "";
        public int CustomizeIcon { get; set; } = 0;
        public int CustomizeIconSelected { get; set; } = 0;
        public int OrderIndex { get; set; } = 1;
        public int CameraZoomLevel { get; set; } = 0;
        public int Flags { get; set; } = 0; 
        public int SpellShapeshiftFormId { get; set; } = 0;
        public decimal CameraDistanceOffset { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
