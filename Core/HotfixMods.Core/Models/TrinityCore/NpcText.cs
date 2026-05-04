using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class NpcText
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public float Probability0 { get; set; } = 0;
        public float Probability1 { get; set; } = 0;
        public float Probability2 { get; set; } = 0;
        public float Probability3 { get; set; } = 0;
        public float Probability4 { get; set; } = 0;
        public float Probability5 { get; set; } = 0;
        public float Probability6 { get; set; } = 0;
        public float Probability7 { get; set; } = 0;
        public uint BroadcastTextID0 { get; set; } = 0;
        public uint BroadcastTextID1 { get; set; } = 0;
        public uint BroadcastTextID2 { get; set; } = 0;
        public uint BroadcastTextID3 { get; set; } = 0;
        public uint BroadcastTextID4 { get; set; } = 0;
        public uint BroadcastTextID5 { get; set; } = 0;
        public uint BroadcastTextID6 { get; set; } = 0;
        public uint BroadcastTextID7 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
