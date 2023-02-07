using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationChoice
    {
        public string Name { get; set; } = "";
        public uint ID { get; set; } = 0;
        public int ChrCustomizationOptionID { get; set; } = 0;
        public int ChrCustomizationReqID { get; set; } = 0;
        public int ChrCustomizationVisReqID { get; set; } = 0;
        public ushort OrderIndex { get; set; } = 0;
        public ushort UiOrderIndex { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int AddedInPatch { get; set; } = 0;
        public int SwatchColor0 { get; set; } = 0;
        public int SwatchColor1 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
