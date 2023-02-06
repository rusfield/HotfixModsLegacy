using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [CharactersSchema]
    public class CharacterInventory
    {
        public ulong Guid { get; set; }
        public ulong Bag { get; set; }
        public byte Slot { get; set; }
        public ulong Item { get; set; }
    }

}
