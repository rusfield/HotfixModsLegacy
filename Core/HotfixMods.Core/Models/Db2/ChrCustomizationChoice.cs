using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ChrCustomizationChoice
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ChrCustomizationOptionId { get; set; }
        public int ChrCustomizationReqId { get; set; }
        public int ChrCustomizationVisReqId { get; set; }
        public ushort OrderIndex { get; set; }
        public ushort UiOrderIndex { get; set; }
        public int Flags { get; set; }
        public int AddedInPatch { get; set; }
        public int SwatchColor1 { get; set; }
        public int SwatchColor2 { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
