using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;


namespace HotfixMods.Core.Models
{
    public class CharacterInventory : ICharactersSchema
    {
        public int Guid { get; set; }
        public CharacterInventorySlots Slot { get; set; }
        public int Item { get; set; }
    }
}
