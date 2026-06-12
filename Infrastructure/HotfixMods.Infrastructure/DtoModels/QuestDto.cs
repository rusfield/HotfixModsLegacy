using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class QuestDto : DtoBase
    {
        public QuestDto() : base("Quest") { }

        public QuestTemplate QuestTemplate { get; set; } = new();
        public QuestOfferReward QuestOfferReward { get; set; } = new();
        public QuestRequestItems QuestRequestItems { get; set; } = new();
        public QuestV2 QuestV2 { get; set; } = new();

        public QuestTemplateAddon? QuestTemplateAddon { get; set; }
        public QuestDetails? QuestDetails { get; set; }

        public List<QuestObjectiveGroup> QuestObjectiveGroups { get; set; } = new();
        public class QuestObjectiveGroup
        {
            public QuestObjectives QuestObjectives { get; set; } = new();
            public QuestObjective QuestObjective { get; set; } = new();
        }

        public List<CreatureQueststarterGroup> CreatureQueststarterGroups { get; set; } = new();
        public class CreatureQueststarterGroup
        {
            public CreatureQueststarter CreatureQueststarter { get; set; } = new();
        }

        public List<GameobjectQueststarterGroup> GameobjectQueststarterGroups { get; set; } = new();
        public class GameobjectQueststarterGroup
        {
            public GameobjectQueststarter GameobjectQueststarter { get; set; } = new();
        }

        public List<GameobjectQuestenderGroup> GameobjectQuestenderGroups { get; set; } = new();
        public class GameobjectQuestenderGroup
        {
            public GameobjectQuestender GameobjectQuestender { get; set; } = new();
        }
    }
}
