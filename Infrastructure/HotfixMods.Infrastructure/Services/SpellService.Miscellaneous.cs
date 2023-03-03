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
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var spellId = await GetIdByConditionsAsync<Spell>(dto.Spell.ID, dto.IsUpdate);
            var spellMiscId = await GetIdByConditionsAsync<SpellMisc>(dto.SpellMisc.ID, dto.IsUpdate);

            var spellCooldownsId = await GetIdByConditionsAsync<SpellCooldowns>(dto.SpellCooldowns?.ID, dto.IsUpdate);
            var spellPowerId = await GetIdByConditionsAsync<SpellPower>(dto.SpellPower?.ID, dto.IsUpdate);
            var spellAuraOptionsId = await GetIdByConditionsAsync<SpellAuraOptions>(dto.SpellAuraOptions?.ID, dto.IsUpdate);
            var spellXSpellVisualId = await GetIdByConditionsAsync<SpellXSpellVisual>(dto.SpellXSpellVisual?.ID, dto.IsUpdate);
            var spellVisualId = await GetIdByConditionsAsync<SpellVisual>(dto.SpellVisual?.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextSpellEffectId = await GetNextIdAsync<SpellEffect>();
            var nextSpellVisualEventId = await GetNextIdAsync<SpellVisualEvent>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = spellId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Spell.ID = spellId;
            dto.Spell.VerifiedBuild = VerifiedBuild;

            dto.SpellName.ID = spellId;
            dto.SpellName.VerifiedBuild= VerifiedBuild;

            dto.SpellMisc.ID = spellMiscId;
            dto.SpellMisc.SpellID = (int)spellId;
            dto.SpellMisc.VerifiedBuild = VerifiedBuild;

            if(dto.SpellXSpellVisual != null)
            {
                dto.SpellXSpellVisual.ID = spellXSpellVisualId;
                dto.SpellXSpellVisual.SpellID = (int)spellId;
                dto.SpellXSpellVisual.SpellVisualID = spellVisualId;
                dto.SpellXSpellVisual.VerifiedBuild = VerifiedBuild;

                if (dto.SpellVisual != null)
                {
                    dto.SpellVisual.ID = spellVisualId;
                    dto.SpellVisual.VerifiedBuild = VerifiedBuild;
                }
            }

            if (dto.SpellCooldowns != null)
            {
                dto.SpellCooldowns.ID = spellCooldownsId;
                dto.SpellCooldowns.SpellID = (int)spellId;
                dto.SpellCooldowns.VerifiedBuild = VerifiedBuild;
            }

            if(dto.SpellPower!= null)
            {
                dto.SpellPower.ID = spellPowerId;
                dto.SpellPower.SpellID = (int)spellId;
                dto.SpellPower.VerifiedBuild= VerifiedBuild;
            }

            if(dto.SpellAuraOptions != null)
            {
                dto.SpellAuraOptions.ID = spellAuraOptionsId;
                dto.SpellAuraOptions.SpellID = (int)spellId;
                dto.SpellAuraOptions.VerifiedBuild = VerifiedBuild;
            }

            int index = 0;
            dto.EffectGroups.ForEach(e =>
            {
                e.SpellEffect.EffectIndex = index++;
                e.SpellEffect.ID = nextSpellEffectId++;
                e.SpellEffect.SpellID = (int)spellId;
                e.SpellEffect.VerifiedBuild = VerifiedBuild;
            });

            dto.EventGroups.ForEach(v =>
            {
                v.SpellVisualEvent.ID = nextSpellVisualEventId++;
                v.SpellVisualEvent.SpellVisualID = (int)spellVisualId;
                v.SpellVisualEvent.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
