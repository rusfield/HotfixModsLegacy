using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService
    {
        async Task SetIdAndVerifiedBuild(SpellDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.Id, dto.IsUpdate);
            var spellId = await GetIdByConditionsAsync<Spell>(dto.Spell.Id, dto.IsUpdate);
            var spellMiscId = await GetIdByConditionsAsync<SpellMisc>(dto.SpellMisc.Id, dto.IsUpdate);

            var spellCooldownsId = await GetIdByConditionsAsync<SpellCooldowns>(dto.SpellCooldowns?.Id, dto.IsUpdate);
            var spellPowerId = await GetIdByConditionsAsync<SpellPower>(dto.SpellPower?.Id, dto.IsUpdate);
            var spellAuraOptionsId = await GetIdByConditionsAsync<SpellAuraOptions>(dto.SpellAuraOptions?.Id, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextSpellEffectId = await GetNextIdAsync<SpellEffect>();
            var nextSpellXSpellVisualId = await GetNextIdAsync<SpellXSpellVisual>();
            var nextSpellVisualId = await GetNextIdAsync<SpellVisual>();
            var nextSpellVisualEventId = await GetNextIdAsync<SpellVisualEvent>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.Id = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordId = spellId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Spell.Id = spellId;
            dto.Spell.VerifiedBuild = VerifiedBuild;

            dto.SpellName.Id = spellId;
            dto.SpellName.VerifiedBuild= VerifiedBuild;

            dto.SpellMisc.Id = spellMiscId;
            dto.SpellMisc.SpellId = (int)spellId;
            dto.SpellMisc.VerifiedBuild = VerifiedBuild;


            if(dto.SpellCooldowns != null)
            {
                dto.SpellCooldowns.Id = spellCooldownsId;
                dto.SpellCooldowns.SpellId = (int)spellId;
                dto.SpellCooldowns.VerifiedBuild = VerifiedBuild;
            }

            if(dto.SpellPower!= null)
            {
                dto.SpellPower.Id = spellPowerId;
                dto.SpellPower.SpellId= (int)spellId;
                dto.SpellPower.VerifiedBuild= VerifiedBuild;
            }

            if(dto.SpellAuraOptions != null)
            {
                dto.SpellAuraOptions.Id = spellAuraOptionsId;
                dto.SpellAuraOptions.SpellId = (int)spellId;
                dto.SpellAuraOptions.VerifiedBuild = VerifiedBuild;
            }

            dto.EffectGroups.ForEach(e =>
            {
                e.SpellEffect.Id = nextSpellEffectId++;
                e.SpellEffect.SpellId = (int)spellId;
                e.SpellEffect.VerifiedBuild = VerifiedBuild;
            });

            dto.VisualGroups.ForEach(v =>
            {
                v.SpellXSpellVisual.Id = nextSpellXSpellVisualId;
                v.SpellXSpellVisual.SpellId = (int)spellId;
                v.SpellXSpellVisual.SpellVisualId = nextSpellVisualId;
                v.SpellXSpellVisual.VerifiedBuild = VerifiedBuild;

                v.SpellVisual.Id = nextSpellVisualId;
                v.SpellVisual.VerifiedBuild = VerifiedBuild;

                v.SpellVisualEvent.Id = nextSpellVisualEventId;
                v.SpellVisualEvent.SpellVisualId = (int)nextSpellVisualId;
                v.SpellVisualEvent.VerifiedBuild = VerifiedBuild;

                nextSpellEffectId++;
                nextSpellVisualEventId++;
                nextSpellVisualId++;
            });
        }
    }
}
