using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class GossipDto : DtoBase
    {
        public GossipDto() : base("Gossip") { }

        public List<MenuGroup> MenuGroups { get; set; } = new();
        public List<OptionGroup> OptionGroups { get; set; } = new();

        public class MenuGroup
        {
            public GossipMenu GossipMenu { get; set; } = new();
            public NpcText NpcText { get; set; } = new();
            public List<GreetingTextGroup> GreetingTextGroups { get; set; } = new();
        }

        public class GreetingTextGroup
        {
            public BroadcastText BroadcastText { get; set; } = new();
        }

        public class OptionGroup
        {
            public GossipMenuOption GossipMenuOption { get; set; } = new();
            public BroadcastText BroadcastText { get; set; } = new();
            public GossipNpcOption GossipNpcOption { get; set; } = new();
        }
    }
}
