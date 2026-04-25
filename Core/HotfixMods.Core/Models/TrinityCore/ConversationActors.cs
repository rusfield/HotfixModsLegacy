using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class ConversationActors
    {
        public int ConversationId { get; set; } = 0;
        public int ConversationActorId { get; set; } = 0;
        public ulong ConversationActorGuid { get; set; } = 0;
        public ushort Idx { get; set; } = 0;
        public int CreatureId { get; set; } = 0;
        public int CreatureDisplayInfoId { get; set; } = 0;
        public byte NoActorObject { get; set; } = 0;
        public byte ActivePlayerObject { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
