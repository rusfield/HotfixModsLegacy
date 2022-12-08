using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class BeamEffect
    {
        public int Id { get; set; } = 1;
        public int BeamId { get; set; } = 0;
        public decimal SourceMinDistance { get; set; } = 0;
        public decimal FixedLength { get; set; } = 0;
        public int Flags { get; set; } = 0;
        public int SourceOffset { get; set; } = 0;
        public int DestOffset { get; set; } = 0;
        public int SourceAttachId { get; set; } = -1;
        public int DestAttachId { get; set; } = -1;
        public int SourcePositionerId { get; set; } = 0;
        public int DestPositionerId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
