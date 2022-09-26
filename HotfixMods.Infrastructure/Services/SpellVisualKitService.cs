using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Core.Providers;
using HotfixMods.Infrastructure.DashboardModels;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellVisualKitService : Service
    {
        public SpellVisualKitService(IDb2Provider db2Provider, IMySqlProvider mySqlProvider) : base(db2Provider, mySqlProvider) { }

        public async Task<List<DashboardModel>> GetDashboardAsync()
        {
            var hotfixModsData = await _mySql.GetAsync<HotfixModsData>(c => c.VerifiedBuild == VerifiedBuild);
            var result = new List<DashboardModel>();
            foreach (var data in hotfixModsData)
            {
                result.Add(new DashboardModel()
                {
                    Id = data.RecordId,
                    Name = data.Name,
                    Comment = data.Comment,
                    AvatarUrl = "TODO"
                });
            }
            // Newest on top
            result.Reverse();
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteFromHotfixesAsync(id);
        }

        public async Task SaveAsync(SpellVisualKitDto dto)
        {
            var hotfixId = await GetNextHotfixIdAsync();
            dto.InitHotfixes(hotfixId, VerifiedBuild);

            if(null == dto.EffectType || dto.EffectType == SpellVisualKitEffectType.NONE)
            {
                throw new Exception("Invalid EffectType");
            }

            await _mySql.AddOrUpdateAsync(BuildHotfixModsData(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellVisualKit(dto));
            await _mySql.AddOrUpdateAsync(BuildSpellVisualKitEffect(dto));

            switch (dto.EffectType)
            {
                case SpellVisualKitEffectType.MODEL_ATTACH:
                    await _mySql.AddOrUpdateAsync(BuildSpellVisualKitModelAttach(dto));
                    await _mySql.AddOrUpdateAsync(BuildSpellVisualEffectName(dto));
                    break;
            }

            await AddHotfixes(dto.GetHotfixes());
        }

        public async Task<SpellVisualKitDto> GetNewAsync(Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            progressCallback("Done", "Returning new Spell Visual Kit", 100);
            return new SpellVisualKitDto()
            {
                Id = await GetNextIdAsync()
            };
        }

        public async Task<SpellVisualKitDto?> GetByIdAsync(int id, Action<string, string, int>? progressCallback = null)
        {
            return new SpellVisualKitDto();
        }

        async Task DeleteFromHotfixesAsync(int id)
        {

        }
    }
}
