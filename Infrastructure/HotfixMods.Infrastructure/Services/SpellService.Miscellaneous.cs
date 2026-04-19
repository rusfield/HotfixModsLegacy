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
            var nextSpellXSpellVisualId = await GetNextIdAsync<SpellXSpellVisual>();
            var nextSpellVisualId = await GetNextIdAsync<SpellVisual>();

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
            dto.SpellMisc.SpellID = spellId;
            dto.SpellMisc.VerifiedBuild = VerifiedBuild;

            dto.VisualGroups.ForEach(visualGroup =>
            {
                visualGroup.SpellXSpellVisual.ID = nextSpellXSpellVisualId++;
                visualGroup.SpellXSpellVisual.SpellID = spellId;
                visualGroup.SpellXSpellVisual.DifficultyID = (byte)dto.SpellMisc.DifficultyID;
                visualGroup.SpellXSpellVisual.ActiveIconFileID = dto.SpellMisc.ActiveIconFileDataID;
                visualGroup.SpellXSpellVisual.SpellIconFileID = dto.SpellMisc.SpellIconFileDataID;
                visualGroup.SpellXSpellVisual.VerifiedBuild = VerifiedBuild;

                if (visualGroup.SpellVisual != null)
                {
                    visualGroup.SpellVisual.ID = nextSpellVisualId++;
                    visualGroup.SpellVisual.VerifiedBuild = VerifiedBuild;
                    visualGroup.SpellXSpellVisual.SpellVisualID = (uint)visualGroup.SpellVisual.ID;
                }

                visualGroup.EventGroups.ForEach(eventGroup =>
                {
                    eventGroup.SpellVisualEvent.ID = nextSpellVisualEventId++;
                    eventGroup.SpellVisualEvent.SpellVisualID = visualGroup.SpellVisual?.ID ?? 0;
                    eventGroup.SpellVisualEvent.VerifiedBuild = VerifiedBuild;
                });
            });

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
                dto.SpellAuraOptions.DifficultyID = dto.SpellMisc.DifficultyID;
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
        }
    }
}
