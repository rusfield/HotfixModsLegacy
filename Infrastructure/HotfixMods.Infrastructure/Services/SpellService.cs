using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;


namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService : ServiceBase
    {
        public SpellService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, AppConfig appConfig) : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, appConfig) { }

        public async Task<SpellDto> GetNewAsync(Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            return new SpellDto();
        }

        public async Task<SpellDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var spell = await GetSingleAsync<Spell>(new DbParameter(nameof(Spell.Id), id));

            if(null == spell)
            {
                return null;
            }

            var result = new SpellDto()
            {
                HotfixModsEntity = await GetExistingOrNewHotfixModsEntity(id),
                Spell = spell,
                SpellAuraOptions = await GetSingleAsync<SpellAuraOptions>(new DbParameter(nameof(SpellAuraOptions.SpellId), id)),
                SpellCooldowns = await GetSingleAsync<SpellCooldowns>(new DbParameter(nameof(SpellCooldowns.SpellId), id)),
                SpellMisc = await GetSingleAsync<SpellMisc>(new DbParameter(nameof(SpellMisc.SpellId), id)),
                SpellName = await GetSingleAsync<SpellName>(new DbParameter(nameof(SpellName.Id), id)),
                SpellPower = await GetSingleAsync<SpellPower>(new DbParameter(nameof(SpellPower.SpellId), id)),
                EffectGroups = new(),
                VisualGroups = new(),
                IsUpdate = true
            };

            var spellEffects = await GetAsync<SpellEffect>(new DbParameter(nameof(SpellEffect.SpellId), id));
            spellEffects.ForEach(s =>
            {
                result.EffectGroups.Add(new SpellDto.EffectGroup()
                {
                    SpellEffect = s
                });
            });

            // TODO: This must be tested. There may be spells where sv, sve or sxsv are null.
            var spellXSpellVisuals = await GetAsync<SpellXSpellVisual>(new DbParameter(nameof(SpellXSpellVisual.SpellId), id));
            spellXSpellVisuals.ForEach(async sxsv =>
            {
                var spellVisualEvents = await GetAsync<SpellVisualEvent>(new DbParameter(nameof(SpellVisualEvent.SpellVisualId), sxsv.SpellVisualId));
                spellVisualEvents.ForEach(async sve =>
                {
                    var spellVisuals = await GetAsync<SpellVisual>(new DbParameter(nameof(SpellVisual.Id), sve.SpellVisualId));
                    spellVisuals.ForEach(sv =>
                    {
                        result.VisualGroups.Add(new()
                        {
                            SpellVisual = sv,
                            SpellVisualEvent = sve,
                            SpellXSpellVisual = sxsv
                        });
                    });
                });
            });

            return result;
        }

        public async Task<bool> SaveAsync(SpellDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            await SetIdAndVerifiedBuild(dto);

            await SaveAsync(dto.HotfixModsEntity);
            await SaveAsync(dto.Spell);
            await SaveAsync(dto.SpellAuraOptions);
            await SaveAsync(dto.SpellCooldowns);
            await SaveAsync(dto.SpellMisc);
            await SaveAsync(dto.SpellName);
            await SaveAsync(dto.SpellPower);
            dto.EffectGroups.ForEach(async e =>
            {
                await SaveAsync(e.SpellEffect);
            });
            dto.VisualGroups.ForEach(async v =>
            {
                await SaveAsync(v.SpellXSpellVisual);
                await SaveAsync(v.SpellVisual);
                await SaveAsync(v.SpellVisualEvent);
            });


            dto.IsUpdate = true;
            return true;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultProgressCallback;

            var dto = await GetByIdAsync(id);
            if(null == dto)
            {
                return false;
            }

            dto.VisualGroups.ForEach(async v =>
            {
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellVisualEvent);
                await DeleteAsync(v.SpellXSpellVisual);
            });
            dto.EffectGroups.ForEach(async e =>
            {
                await DeleteAsync(e.SpellEffect);
            });
            await DeleteAsync(dto.SpellPower);
            await DeleteAsync(dto.SpellName);
            await DeleteAsync(dto.SpellMisc);
            await DeleteAsync(dto.SpellCooldowns);
            await DeleteAsync(dto.SpellAuraOptions);
            await DeleteAsync(dto.Spell);
            await DeleteAsync(dto.HotfixModsEntity);

            return true;
        }
    }
}
