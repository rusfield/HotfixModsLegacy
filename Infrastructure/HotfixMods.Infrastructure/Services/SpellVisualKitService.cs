using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public class SpellVisualKitService : ServiceBase
    {
        public SpellVisualKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }
        public SpellVisualKitDto GetNew(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;
            callback.Invoke(LoadingHelper.Loading, "Returning new template", 100);
            return new();
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            var dtos = await GetAsync<HotfixModsEntity>(new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
            var results = new List<DashboardModel>();
            foreach (var dto in dtos)
            {
                results.Add(new()
                {
                    Id = dto.RecordId,
                    Name = dto.Name,
                    AvatarUrl = null
                });
            }
            return results;
        }

        public async Task<SpellVisualKitDto?> GetByIdAsync(uint id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var spellVisualKit = await GetSingleAsync<SpellVisualKit>(new DbParameter(nameof(SpellVisualKit.Id), id));
            if (null == spellVisualKit)
            {
                return null;
            }
            return new SpellVisualKitDto()
            {
                SpellVisualKit = spellVisualKit,
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(spellVisualKit.Id)
            };
        }

        public async Task<bool> SaveAsync(SpellVisualKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SaveAsync(dto.SpellVisualKit);
            await SaveAsync(dto.HotfixModsEntity);

            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(uint id, Action<string, string, int>? callback = null)
        {
            await DeleteAsync<SpellVisualKit>(new DbParameter(nameof(AnimKit.Id), id));
            // TODO: HotfixMods
            return false;
        }

        public async Task<uint> GetNextIdAsync()
        {
            return await GetNextIdAsync<SpellVisualKit>();
        }
    }
}
