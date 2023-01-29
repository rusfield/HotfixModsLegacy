using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationChoice
    {
        public string Name { get; set; } = "";
        public uint Id { get; set; } = 0;
        public int ChrCustomizationOptionId { get; set; } = 0;
        public int ChrCustomizationReqId { get; set; } = 0;
        public int ChrCustomizationVisReqId { get; set; } = 0;
        public ushort OrderIndex { get; set; } = 0;
        public ushort UiOrderIndex { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int AddedInPatch { get; set; } = 0;
        public int SwatchColor1 { get; set; } = 0;
        public int SwatchColor2 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
