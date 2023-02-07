using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BeamEffect
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public int BeamID { get; set; } = 0;
        public decimal SourceMinDistance { get; set; } = 0;
        public decimal FixedLength { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int SourceOffset { get; set; } = 0;
        public int DestOffset { get; set; } = 0;
        public int SourceAttachID { get; set; } = -1;
        public int DestAttachID { get; set; } = -1;
        public int SourcePositionerID { get; set; } = 0;
        public int DestPositionerID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
