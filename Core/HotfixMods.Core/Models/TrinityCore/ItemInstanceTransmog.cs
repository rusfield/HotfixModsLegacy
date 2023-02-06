using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{
    [CharactersSchema]
    public class ItemInstanceTransmog
    {
        public ulong ItemGuid { get; set; }
        public int ItemModifiedAppearanceAllSpecs { get; set; }
        public int ItemModifiedAppearanceSpec1 { get; set; }
        public int ItemModifiedAppearanceSpec2 { get; set; }
        public int ItemModifiedAppearanceSpec3 { get; set; }
        public int ItemModifiedAppearanceSpec4 { get; set; }
        public int ItemModifiedAppearanceSpec5 { get; set; }
        public int SpellItemEnchantmentAllSpecs { get; set; }
        public int SpellItemEnchantmentSpec1 { get; set; }
        public int SpellItemEnchantmentSpec2 { get; set; }
        public int SpellItemEnchantmentSpec3 { get; set; }
        public int SpellItemEnchantmentSpec4 { get; set; }
        public int SpellItemEnchantmentSpec5 { get; set; }
        public int SecondaryItemModifiedAppearanceAllSpecs { get; set; }
        public int SecondaryItemModifiedAppearanceSpec1 { get; set; }
        public int SecondaryItemModifiedAppearanceSpec2 { get; set; }
        public int SecondaryItemModifiedAppearanceSpec3 { get; set; }
        public int SecondaryItemModifiedAppearanceSpec4 { get; set; }
        public int SecondaryItemModifiedAppearanceSpec5 { get; set; }
    }

}
