using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellDto : DtoBase
    {
        public SpellDto() : base(nameof(Spell)) { }

        public Spell Spell { get; set; } = new();
        public SpellAuraOptions SpellAuraOptions { get; set; } = new();
        public SpellCooldowns SpellCooldowns { get; set; } = new();
        public SpellMisc SpellMisc { get; set; } = new();
        public SpellName SpellName { get; set; } = new();
        public SpellPower SpellPower { get; set; } = new();
        public List<EffectGroup> EffectGroups { get; set; } = new();
        public List<VisualGroup> VisualGroups { get; set; } = new();

        public class EffectGroup
        {
            public SpellEffect SpellEffect { get; set; } = new();
        }

        public class VisualGroup
        {
            public SpellXSpellVisual SpellXSpellVisual { get; set; } = new();
            public SpellVisual SpellVisual { get; set; } = new();
            public SpellVisualEvent SpellVisualEvent { get; set; } = new();
        }
    }
}
