using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BeamEffect
    {
        public int Id { get; set; } = 1;
        public int BeamId { get; set; }
        public decimal SourceMinDistance { get; set; }
        public decimal FixedLength { get; set; }
        public int Flags { get; set; }
        public int SourceOffset { get; set; }
        public int DestOffset { get; set; }
        public int SourceAttachId { get; set; }
        public int DestAttachId { get; set; }
        public int SourcePositionerId { get; set; }
        public int DestPositionerId { get; set; }
        public int VerifiedBuild { get; set; } = -1;
    }
}
