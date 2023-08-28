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
            var spellId = await GetIdByConditionsAsync<Spell>((ulong)dto.Spell.ID, dto.IsUpdate);
            var spellMiscId = await GetIdByConditionsAsync<SpellMisc>((ulong)dto.SpellMisc.ID, dto.IsUpdate);

            var spellCooldownsId = await GetIdByConditionsAsync<SpellCooldowns>((ulong?)dto.SpellCooldowns?.ID, dto.IsUpdate);
            var spellPowerId = await GetIdByConditionsAsync<SpellPower>((ulong?)dto.SpellPower?.ID, dto.IsUpdate);
            var spellAuraOptionsId = await GetIdByConditionsAsync<SpellAuraOptions>((ulong?)dto.SpellAuraOptions?.ID, dto.IsUpdate);
            var spellXSpellVisualId = await GetIdByConditionsAsync<SpellXSpellVisual>((ulong?)dto.SpellXSpellVisual?.ID, dto.IsUpdate);
            var spellVisualId = await GetIdByConditionsAsync<SpellVisual>((ulong?)dto.SpellVisual?.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextSpellEffectId = await GetNextIdAsync<SpellEffect>();
            var nextSpellVisualEventId = await GetNextIdAsync<SpellVisualEvent>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = spellId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Spell.ID = (int)spellId;
            dto.Spell.VerifiedBuild = VerifiedBuild;

            dto.SpellName.ID = (int)spellId;
            dto.SpellName.VerifiedBuild= VerifiedBuild;

            dto.SpellMisc.ID = (int)spellMiscId;
            dto.SpellMisc.SpellID = (int)spellId;
            dto.SpellMisc.VerifiedBuild = VerifiedBuild;

            if(dto.SpellXSpellVisual != null)
            {
                dto.SpellXSpellVisual.ID = (int)spellXSpellVisualId;
                dto.SpellXSpellVisual.SpellID = (int)spellId;
                dto.SpellXSpellVisual.SpellVisualID = (uint)spellVisualId;
                dto.SpellXSpellVisual.ActiveIconFileID = dto.SpellMisc.ActiveIconFileDataID;
                dto.SpellXSpellVisual.SpellIconFileID = dto.SpellMisc.SpellIconFileDataID;
                dto.SpellXSpellVisual.DifficultyID = dto.SpellMisc.DifficultyID;
                dto.SpellXSpellVisual.VerifiedBuild = VerifiedBuild;

                if (dto.SpellVisual != null)
                {
                    dto.SpellVisual.ID = (int)spellVisualId;
                    dto.SpellVisual.VerifiedBuild = VerifiedBuild;
                }
            }

            if (dto.SpellCooldowns != null)
            {
                dto.SpellCooldowns.ID = (int)spellCooldownsId;
                dto.SpellCooldowns.SpellID = (int)spellId;
                dto.SpellCooldowns.VerifiedBuild = VerifiedBuild;
            }

            if(dto.SpellPower!= null)
            {
                dto.SpellPower.ID = (int)spellPowerId;
                dto.SpellPower.SpellID = (int)spellId;
                dto.SpellPower.VerifiedBuild= VerifiedBuild;
            }

            if(dto.SpellAuraOptions != null)
            {
                dto.SpellAuraOptions.ID = (int)spellAuraOptionsId;
                dto.SpellAuraOptions.SpellID = (int)spellId;
                dto.SpellAuraOptions.DifficultyID = dto.SpellMisc.DifficultyID;
                dto.SpellAuraOptions.VerifiedBuild = VerifiedBuild;
            }

            int index = 0;
            dto.EffectGroups.ForEach(e =>
            {
                e.SpellEffect.EffectIndex = index++;
                e.SpellEffect.ID = (int)nextSpellEffectId++;
                e.SpellEffect.SpellID = (int)spellId;
                e.SpellEffect.VerifiedBuild = VerifiedBuild;
            });

            dto.EventGroups.ForEach(v =>
            {
                v.SpellVisualEvent.ID = (int)nextSpellVisualEventId++;
                v.SpellVisualEvent.SpellVisualID = (int)spellVisualId;
                v.SpellVisualEvent.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
