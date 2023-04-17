using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models;

namespace HotfixMods.Infrastructure.Services
{
    public partial class GameobjectService
    {
        #region GameobjectTemplate

        public async Task<Dictionary<byte, string>> GetTypeOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(GameobjectTemplate), nameof(GameobjectTemplate.Type));
        }

        public async Task<Dictionary<string, string>> GetIconNameOptionsAsync()
        {
            return Enum.GetValues<IconNames>().ToDictionary(key => key.ToString(), value => value.ToString());
        }

        public async Task<Dictionary<string, string>> GetAiNameOptionsAsync()
        {
            return Enum.GetValues<GameobjectTemplateAiNames>().ToDictionary(key => key.ToString(), value => value.ToString());
        }

        #endregion

        #region GameobjectTemplateAddon
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            return await GetFactionOptionsAsync<ushort>();
        }

        public async Task<Dictionary<uint, string>> GetFlagsOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(GameobjectTemplateAddon), nameof(GameobjectTemplateAddon.Flags));
        }
        #endregion
    }
}
