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

            if (null == dto.EffectType || dto.EffectType == SpellVisualKitEffectType.NONE)
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
            var hmData = await _mySql.GetSingleAsync<HotfixModsData>(h => h.RecordId == id && h.VerifiedBuild == VerifiedBuild);
            var spellVisualKit = await _mySql.GetSingleAsync<SpellVisualKit>(s => s.Id == id) ?? await _db2.GetSingleAsync<SpellVisualKit>(s => s.Id == id);
            var spellVisualKitEffect = await _mySql.GetSingleAsync<SpellVisualKitEffect>(s => s.Id == id) ?? await _db2.GetSingleAsync<SpellVisualKitEffect>(s => s.Id == id);

            if (null == spellVisualKit && null == hmData)
            {
                return null;
            }
            else if (null == spellVisualKit && hmData != null)
            {
                return new()
                {
                    Id = hmData.Id,
                    HotfixModsName = hmData.Name
                };
            }

            var result = new SpellVisualKitDto()
            {
                Id = hmData != null ? id : await GetNextIdAsync(),
                IsUpdate = hmData != null,
                HotfixModsName = hmData?.Name,
                HotfixModsComment = hmData?.Comment,
                EffectType = spellVisualKitEffect?.EffectType,
                Flags0 = spellVisualKit!.Flags0,
                Flags1 = spellVisualKit.Flags1,
                ClutterLevel = spellVisualKit.ClutterLevel,
                FallbackSpellVisualKitId = spellVisualKit.FallbackSpellVisualKitId,
                DelayMax = spellVisualKit.DelayMax,
                DelayMin = spellVisualKit.DelayMin 
            };

            switch (spellVisualKitEffect?.EffectType)
            {
                case SpellVisualKitEffectType.MODEL_ATTACH:
                    var spellVisualKitModelAttach = await _mySql.GetSingleAsync<SpellVisualKitModelAttach>(s => s.ParentSpellVisualKitId == id) ?? await _db2.GetSingleAsync<SpellVisualKitModelAttach>(s => s.ParentSpellVisualKitId == id);
                    SpellVisualEffectName? spellVisualEffectName = null;
                    if(spellVisualKitModelAttach != null)
                        spellVisualEffectName = await _mySql.GetSingleAsync<SpellVisualEffectName>(s => s.Id == spellVisualKitModelAttach.SpellVisualEffectNameId) ?? await _db2.GetSingleAsync<SpellVisualEffectName>(s => s.Id == spellVisualKitModelAttach.SpellVisualEffectNameId);

                    result.Alpha = spellVisualEffectName?.Alpha;
                    result.Scale = spellVisualEffectName?.Scale;
                    result.GenericId = spellVisualEffectName?.GenericId;
                    result.ModelFileDataId = spellVisualEffectName?.ModelFileDataId;
                    result.TextureFileDataId = spellVisualEffectName?.TextureFileDataId;
                    result.AttachmentId = spellVisualKitModelAttach?.AttachmentId;
                    result.Type = spellVisualEffectName?.Type;
                    result.Offset0 = spellVisualKitModelAttach?.Offset0;
                    result.Offset1 = spellVisualKitModelAttach?.Offset1;
                    result.Offset2 = spellVisualKitModelAttach?.Offset2;
                    result.Yaw = spellVisualKitModelAttach?.Yaw;
                    result.Pitch = spellVisualKitModelAttach?.Pitch;
                    result.Roll = spellVisualKitModelAttach?.Roll;
                    result.StartDelay = spellVisualKitModelAttach?.StartDelay;
                    result.ModelAttachFlags = spellVisualKitModelAttach?.Flags;
                    result.SpellVisualEffectNameFlags = spellVisualEffectName?.Flags;
                    result.BaseMissileSpeed = spellVisualEffectName?.BaseMissileSpeed;
                    result.EffectRadius = spellVisualEffectName?.EffectRadius;
                    result.PositionerId = spellVisualKitModelAttach?.PositionerId;
                    result.DissolveEffectId = spellVisualEffectName?.DissolveEffectId;
                    result.RibbonQualityId = spellVisualEffectName?.RibbonQualityId;
                    result.StartAnimId = spellVisualKitModelAttach?.StartAnimId;
                    result.AnimId = spellVisualKitModelAttach?.AnimId;
                    result.EndAnimId = spellVisualKitModelAttach?.EndAnimId;
                    result.AnimKitId = spellVisualKitModelAttach?.AnimKitId;

                    break;
            }
            return result;
        }

        async Task DeleteFromHotfixesAsync(int id)
        {

        }
    }
}
