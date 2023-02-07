using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{
    [CharactersSchema]
    public class ItemInstance
    {
        public ulong Guid { get; set; }
        public uint ItemEntry { get; set; }
        public ulong Owner_Guid { get; set; }
        public ulong CreatorGuid { get; set; }
        public ulong GiftCreatorGuid { get; set; }
        public uint Count { get; set; }
        public int Duration { get; set; }
        public string Charges { get; set; }
        public uint Flags { get; set; }
        public string Enchantments { get; set; }
        public uint RandomBonusListID { get; set; }
        public ushort Durability { get; set; }
        public uint PlayedTime { get; set; }
        public string Text { get; set; }
        public uint Transmogrification { get; set; }
        public uint EnchantIllusion { get; set; }
        public uint BattlePetSpeciesID { get; set; }
        public uint BattlePetBreedData { get; set; }
        public ushort BattlePetLevel { get; set; }
        public uint BattlePetDisplayID { get; set; }
        public byte Context { get; set; }
        public string BonusListIds { get; set; }
    }

}
