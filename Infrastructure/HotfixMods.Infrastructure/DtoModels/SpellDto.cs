using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellDto : BaseDto
    {
        public SpellDto() : base(nameof(Spell)) { }

        public Spell Spell { get; set; } = new();
        public SpellAuraOptions SpellAuraOptions { get; set; } = new();
        public SpellCooldowns SpellCooldowns { get; set; } = new();
        public SpellMisc SpellMisc { get; set; } = new();
        public SpellName SpellName { get; set; } = new();
        public SpellPower SpellPower { get; set; } = new();
        public List<SpellEffect> SpellEffects { get; set; } = new();
        public List<SpellXSpellVisual> SpellXSpellVisuals { get; set; } = new();

        // These will be separate
        // public SpellVisual SpellVisual { get; set; } = new();
        // public SpellVisualEvent SpellVisualEvent { get; set; } = new();
    }
}
