using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService
    {
        async Task SetIdAndVerifiedBuild(SpellDto dto)
        {
            if (!dto.IsUpdate)
            {
                var newSpellId = await GetNextIdAsync<Spell>();
                var newSpellEffectId = await GetNextIdAsync<SpellEffect>();
                var newSpellVisualId = await GetNextIdAsync<SpellVisual>();
                var newSpellXSpellVisualId = await GetNextIdAsync<SpellXSpellVisual>();
                var newSpellVisualEventId = await GetNextIdAsync<SpellVisualEvent>();

                dto.HotfixModsEntity.Id = await GetNextIdAsync<HotfixModsEntity>();
                dto.HotfixModsEntity.RecordId = newSpellId;
                dto.Spell.Id = newSpellId;

                dto.SpellAuraOptions.Id = await GetNextIdAsync<SpellAuraOptions>();
                dto.SpellAuraOptions.SpellId = newSpellId;

                dto.SpellCooldowns.Id = await GetNextIdAsync<SpellAuraOptions>();
                dto.SpellCooldowns.SpellId = newSpellId;

                dto.SpellMisc.Id = await GetNextIdAsync<SpellMisc>();
                dto.SpellMisc.SpellId = newSpellId;

                dto.SpellName.Id = newSpellId;

                dto.SpellPower.Id = await GetNextIdAsync<SpellPower>();
                dto.SpellPower.SpellId = newSpellId;

                dto.EffectGroups.ForEach(s =>
                {
                    s.SpellEffect.Id = newSpellEffectId;
                    s.SpellEffect.SpellId = newSpellId;

                    newSpellEffectId++;
                });

                dto.VisualGroups.ForEach(v =>
                {
                    v.SpellXSpellVisual.Id = newSpellXSpellVisualId;
                    v.SpellXSpellVisual.SpellId = newSpellId;
                    v.SpellXSpellVisual.SpellVisualId = (uint)newSpellVisualId;

                    v.SpellVisual.Id = newSpellVisualId;

                    v.SpellVisualEvent.Id = newSpellVisualEventId;
                    v.SpellVisualEvent.SpellVisualId = newSpellVisualId;

                    newSpellXSpellVisualId++;
                    newSpellVisualId++;
                    newSpellVisualEventId++;
                });
            }

            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;
            dto.Spell.VerifiedBuild = VerifiedBuild;
            dto.SpellAuraOptions.VerifiedBuild = VerifiedBuild;
            dto.SpellCooldowns.VerifiedBuild = VerifiedBuild;
            dto.SpellMisc.VerifiedBuild = VerifiedBuild;
            dto.SpellName.VerifiedBuild = VerifiedBuild;
            dto.SpellPower.VerifiedBuild = VerifiedBuild;
            dto.EffectGroups.ForEach(e =>
            {
                e.SpellEffect.VerifiedBuild = VerifiedBuild;
            });
            dto.VisualGroups.ForEach(v =>
            {
                v.SpellXSpellVisual.VerifiedBuild = VerifiedBuild;
                v.SpellVisual.VerifiedBuild = VerifiedBuild;
                v.SpellVisualEvent.VerifiedBuild = VerifiedBuild;
            });
        }
    }
}
