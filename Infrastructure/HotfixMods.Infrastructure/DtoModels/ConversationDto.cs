using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class ConversationDto : DtoBase
    {
        public ConversationDto() : base(nameof(ConversationTemplate)) { }

        public ConversationTemplate ConversationTemplate { get; set; } = new();
        public List<ActorGroup> ActorGroups { get; set; } = new();
        public List<LineGroup> LineGroups { get; set; } = new();

        public class ActorGroup
        {
            public ConversationActors ConversationActors { get; set; } = new();
        }

        public class LineGroup
        {
            public ConversationLineTemplate ConversationLineTemplate { get; set; } = new();
            public ConversationLine ConversationLine { get; set; } = new();
            public BroadcastText BroadcastText { get; set; } = new();
        }
    }
}
