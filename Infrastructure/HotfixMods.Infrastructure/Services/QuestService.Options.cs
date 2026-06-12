using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Flags.TrinityCore;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class QuestService
    {
        // QuestTemplate
        public async Task<Dictionary<byte, string>> GetQuestTypeOptionsAsync()
        {
            return Enum.GetValues<QuestType>()
                .ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<uint, string>> GetQuestFlagsOptionsAsync()
        {
            return Enum.GetValues<QuestFlags>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<uint, string>> GetQuestFlagsExOptionsAsync()
        {
            return Enum.GetValues<QuestFlagsEx>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<uint, string>> GetQuestFlagsEx2OptionsAsync()
        {
            return Enum.GetValues<QuestFlagsEx2>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<ulong, string>> GetQuestAllowableRacesOptionsAsync()
        {
            return Enum.GetValues<QuestAllowableRaces>()
                .ToDictionary(key => (ulong)key, value => value.ToDisplayString());
        }

        // QuestTemplateAddon
        public async Task<Dictionary<uint, string>> GetQuestAllowableClassesOptionsAsync()
        {
            return Enum.GetValues<QuestAllowableClasses>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<byte, string>> GetQuestSpecialFlagsOptionsAsync()
        {
            return Enum.GetValues<QuestSpecialFlags>()
                .ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }

        // QuestObjectives
        public async Task<Dictionary<byte, string>> GetQuestObjectiveTypeOptionsAsync()
        {
            return Enum.GetValues<QuestObjectiveType>()
                .ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<uint, string>> GetQuestObjectiveFlagsOptionsAsync()
        {
            return Enum.GetValues<QuestObjectiveFlags>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<uint, string>> GetQuestObjectiveFlags2OptionsAsync()
        {
            return Enum.GetValues<QuestObjectiveFlags2>()
                .ToDictionary(key => (uint)key, value => value.ToDisplayString());
        }
    }
}
