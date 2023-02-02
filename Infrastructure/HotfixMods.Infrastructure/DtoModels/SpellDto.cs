using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SpellDto : DtoBase
    {
        public SpellDto() : base(nameof(Spell)) { }

        public Spell Spell { get; set; } = new();
        public SpellMisc SpellMisc { get; set; } = new();
        public SpellName SpellName { get; set; } = new();
        public SpellAuraOptions? SpellAuraOptions { get; set; }
        public SpellPower? SpellPower { get; set; }
        public SpellCooldowns? SpellCooldowns { get; set; }
        public SpellXSpellVisual? SpellXSpellVisual { get; set; }
        public SpellVisual? SpellVisual { get; set; } 
        public List<EffectGroup> EffectGroups { get; set; } = new();
        public List<EventGroup> EventGroups { get; set; } = new();

        public class EffectGroup
        {
            public SpellEffect SpellEffect { get; set; } = new();
        }

        public class EventGroup
        {
            public SpellVisualEvent SpellVisualEvent { get; set; } = new();
        }
    }
}
