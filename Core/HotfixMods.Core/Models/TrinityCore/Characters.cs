using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    // Helper model to load characters by name
    [CharactersSchema]
    public class Characters
    {
        public ulong Guid { get; set; }
        public uint Account { get; set; }
        public string Name { get; set; }
        public byte Slot { get; set; }
        public byte Race { get; set; }
        public byte Class { get; set; }
        public byte Gender { get; set; }
        public byte Level { get; set; }
        public uint Xp { get; set; }
        public ulong Money { get; set; }
        public byte InventorySlots { get; set; }
        public byte BankSlots { get; set; }
        public byte RestState { get; set; }
        public uint PlayerFlags { get; set; }
        public uint PlayerFlagsEx { get; set; }
        public decimal Position_X { get; set; }
        public decimal Position_Y { get; set; }
        public decimal Position_Z { get; set; }
        public ushort Map { get; set; }
        public uint Instance_Id { get; set; }
        public byte DungeonDifficulty { get; set; }
        public byte RaidDifficulty { get; set; }
        public byte LegacyRaidDifficulty { get; set; }
        public decimal Orientation { get; set; }
        public string Taximask { get; set; }
        public byte Online { get; set; }
        public long CreateTime { get; set; }
        public sbyte CreateMode { get; set; }
        public byte Cinematic { get; set; }
        public uint Totaltime { get; set; }
        public uint Leveltime { get; set; }
        public long Logout_Time { get; set; }
        public byte Is_Logout_Resting { get; set; }
        public decimal Rest_Bonus { get; set; }
        public uint Resettalents_Cost { get; set; }
        public long Resettalents_Time { get; set; }
        public byte NumRespecs { get; set; }
        public uint PrimarySpecialization { get; set; }
        public decimal Trans_X { get; set; }
        public decimal Trans_Y { get; set; }
        public decimal Trans_Z { get; set; }
        public decimal Trans_O { get; set; }
        public ulong Transguid { get; set; }
        public ushort Extra_Flags { get; set; }
        public uint SummonedPetNumber { get; set; }
        public ushort At_Login { get; set; }
        public ushort Zone { get; set; }
        public long Death_Expire_Time { get; set; }
        public string Taxi_Path { get; set; }
        public uint TotalKills { get; set; }
        public ushort TodayKills { get; set; }
        public ushort YesterdayKills { get; set; }
        public uint ChosenTitle { get; set; }
        public uint WatchedFaction { get; set; }
        public byte Drunk { get; set; }
        public uint Health { get; set; }
        public uint Power1 { get; set; }
        public uint Power2 { get; set; }
        public uint Power3 { get; set; }
        public uint Power4 { get; set; }
        public uint Power5 { get; set; }
        public uint Power6 { get; set; }
        public uint Power7 { get; set; }
        public uint Latency { get; set; }
        public byte ActiveTalentGroup { get; set; }
        public uint LootSpecId { get; set; }
        public string ExploredZones { get; set; }
        public string EquipmentCache { get; set; }
        public string KnownTitles { get; set; }
        public byte ActionBars { get; set; }
        public uint DeleteInfos_Account { get; set; }
        public string DeleteInfos_Name { get; set; }
        public long DeleteDate { get; set; }
        public uint Honor { get; set; }
        public uint HonorLevel { get; set; }
        public byte HonorRestState { get; set; }
        public decimal HonorRestBonus { get; set; }
        public uint LastLoginBuild { get; set; }
    }

}
