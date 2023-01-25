using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSparse
    {
        [IndexField]
        public int Id { get; set; } = 0;
        public long AllowableRace { get; set; } = 0;
        [LocalizedString]
        public string Description { get; set; } = "";
        [LocalizedString]
        public string Display3 { get; set; } = "";
        [LocalizedString]
        public string Display2 { get; set; } = "";
        [LocalizedString]
        public string Display1 { get; set; } = "";
        [LocalizedString]
        public string Display { get; set; } = "";
        public int ExpansionId { get; set; } = 0;
        public decimal DmgVariance { get; set; } = 0;
        public int LimitCategory { get; set; } = 0;
        public uint DurationInInventory { get; set; } = 0;
        public decimal QualityModifier { get; set; } = 0;
        public uint BagFamily { get; set; } = 0;
        public int StartQuestId { get; set; } = 0;
        public int LanguageId { get; set; } = 0;
        public decimal ItemRange { get; set; } = 0;
        public decimal StatPercentageOfSocket1 { get; set; } = 0;
        public decimal StatPercentageOfSocket2 { get; set; } = 0;
        public decimal StatPercentageOfSocket3 { get; set; } = 0;
        public decimal StatPercentageOfSocket4 { get; set; } = 0;     
        public decimal StatPercentageOfSocket5 { get; set; } = 0;
        public decimal StatPercentageOfSocket6 { get; set; } = 0;
        public decimal StatPercentageOfSocket7 { get; set; } = 0;
        public decimal StatPercentageOfSocket8 { get; set; } = 0;
        public decimal StatPercentageOfSocket9 { get; set; } = 0;
        public decimal StatPercentageOfSocket10 { get; set; } = 0;
        public int StatPercentEditor1 { get; set; } = 0;
        public int StatPercentEditor2 { get; set; } = 0;
        public int StatPercentEditor3 { get; set; } = 0;
        public int StatPercentEditor4 { get; set; } = 0;
        public int StatPercentEditor5 { get; set; } = 0;
        public int StatPercentEditor6 { get; set; } = 0;
        public int StatPercentEditor7 { get; set; } = 0;
        public int StatPercentEditor8 { get; set; } = 0;
        public int StatPercentEditor9 { get; set; } = 0;
        public int StatPercentEditor10 { get; set; } = 0;
        public int Stackable { get; set; } = 1;
        public int MaxCount { get; set; } = 0;
        public int MinReputation { get; set; } = 0;
        public uint RequiredAbility { get; set; } = 0;
        public uint SellPrice { get; set; } = 0;
        public uint BuyPrice { get; set; } = 0;
        public uint VendorStackCount { get; set; } = 0;
        public decimal PriceVariance { get; set; } = 0;
        public decimal PriceRandomValue { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int Flags3 { get; set; } = 0;
        public int Flags4 { get; set; } = 0;
        public int OppositeFactionItemId { get; set; } = 0;
        public int ModifiedCraftingReagentItemId { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public int PlayerLevelToItemLevelCurveId { get; set; } = 0;
        public ushort ItemNameDescriptionId { get; set; } = 0;
        public ushort RequiredTransmogHoliday { get; set; } = 0;
        public ushort RequiredHoliday { get; set; } = 0;
        public ushort Gem_properties { get; set; } = 0;
        public ushort Socket_match_enchantment_Id { get; set; } = 0;
        public ushort TotemCategoryId { get; set; } = 0;
        public ushort InstanceBound { get; set; } = 0;
        public ushort ZoneBound1 { get; set; } = 0;
        public ushort ZoneBound2 { get; set; } = 0;
        public ushort ItemSet { get; set; } = 0;
        public ushort LockId { get; set; } = 0;
        public ushort PageId { get; set; } = 0;
        public ushort ItemDelay { get; set; } = 0;
        public ushort MinFactionId { get; set; } = 0;
        public ushort RequiredSkillRank { get; set; } = 0;
        public ushort RequiredSkill { get; set; } = 0;
        public ushort ItemLevel { get; set; } = 0;
        public short AllowableClass { get; set; } = 0;
        public byte ArtifactId { get; set; } = 0;
        public byte SpellWeight { get; set; } = 0;
        public byte SpellWeightCategory { get; set; } = 0;
        public byte SocketType1 { get; set; } = 0;
        public byte SocketType2 { get; set; } = 0;
        public byte SocketType3 { get; set; } = 0;
        public byte SheatheType { get; set; } = 0;
        public byte Material { get; set; } = 0;
        public byte PageMaterialId { get; set; } = 0;
        public byte Bonding { get; set; } = 0;
        public byte DamageType { get; set; } = 0;
        public sbyte StatModifier_bonusStat1 { get; set; } = 0;
        public sbyte StatModifier_bonusStat2 { get; set; } = 0;
        public sbyte StatModifier_bonusStat3 { get; set; } = 0;
        public sbyte StatModifier_bonusStat4 { get; set; } = 0;
        public sbyte StatModifier_bonusStat5 { get; set; } = 0;
        public sbyte StatModifier_bonusStat6 { get; set; } = 0;
        public sbyte StatModifier_bonusStat7 { get; set; } = 0;
        public sbyte StatModifier_bonusStat8 { get; set; } = 0;
        public sbyte StatModifier_bonusStat9 { get; set; } = 0;
        public sbyte StatModifier_bonusStat10 { get; set; } = 0;
        public byte ContainerSlots { get; set; } = 0;
        public byte RequiredPVPMedal { get; set; } = 0;
        public byte RequiredPVPRank { get; set; } = 0;
        public sbyte RequiredLevel { get; set; } = 0;
        public sbyte InventoryType { get; set; } = 0;
        public sbyte OverallQualityId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
