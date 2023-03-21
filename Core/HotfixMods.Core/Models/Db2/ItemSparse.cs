using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemSparse
    {
        [IndexField]
        public int ID { get; set; } = 0;
        [Db2Description("The race combination that can equip this item.$There seems to be some special reserved values for All, Any Horde Race and Any Alliance Race. Combining these options with others may not work.")]
        public long AllowableRace { get; set; } = -1;
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
        public int ExpansionID { get; set; } = 0;
        public decimal DmgVariance { get; set; } = 0;
        public int LimitCategory { get; set; } = 0;
        public uint DurationInInventory { get; set; } = 0;
        public decimal QualityModifier { get; set; } = 0;
        public uint BagFamily { get; set; } = 0;
        public int StartQuestID { get; set; } = 0;
        public int LanguageID { get; set; } = 0;
        public decimal ItemRange { get; set; } = 0;
        public decimal StatPercentageOfSocket0 { get; set; } = 0;
        public decimal StatPercentageOfSocket1 { get; set; } = 0;
        public decimal StatPercentageOfSocket2 { get; set; } = 0;
        public decimal StatPercentageOfSocket3 { get; set; } = 0;     
        public decimal StatPercentageOfSocket4 { get; set; } = 0;
        public decimal StatPercentageOfSocket5 { get; set; } = 0;
        public decimal StatPercentageOfSocket6 { get; set; } = 0;
        public decimal StatPercentageOfSocket7 { get; set; } = 0;
        public decimal StatPercentageOfSocket8 { get; set; } = 0;
        public decimal StatPercentageOfSocket9 { get; set; } = 0;
        public int StatPercentEditor0 { get; set; } = 0;
        public int StatPercentEditor1 { get; set; } = 0;
        public int StatPercentEditor2 { get; set; } = 0;
        public int StatPercentEditor3 { get; set; } = 0;
        public int StatPercentEditor4 { get; set; } = 0;
        public int StatPercentEditor5 { get; set; } = 0;
        public int StatPercentEditor6 { get; set; } = 0;
        public int StatPercentEditor7 { get; set; } = 0;
        public int StatPercentEditor8 { get; set; } = 0;
        public int StatPercentEditor9 { get; set; } = 0;
        public int Stackable { get; set; } = 1;
        public int MaxCount { get; set; } = 0;
        public int MinReputation { get; set; } = 0;
        public uint RequiredAbility { get; set; } = 0;
        public uint SellPrice { get; set; } = 0;
        public uint BuyPrice { get; set; } = 0;
        public uint VendorStackCount { get; set; } = 0;
        public decimal PriceVariance { get; set; } = 0;
        public decimal PriceRandomValue { get; set; } = 0;
        public int Flags0 { get; set; } = 0;
        public int Flags1 { get; set; } = 0;
        public int Flags2 { get; set; } = 0;
        public int Flags3 { get; set; } = 0;
        public int OppositeFactionItemID { get; set; } = 0;
        public int ModifiedCraftingReagentItemID { get; set; } = 0;
        public int ContentTuningID { get; set; } = 0;
        public int PlayerLevelToItemLevelCurveID { get; set; } = 0;
        public ushort ItemNameDescriptionID { get; set; } = 0;
        public ushort RequiredTransmogHoliday { get; set; } = 0;
        public ushort RequiredHoliday { get; set; } = 0;
        public ushort GemProperties { get; set; } = 0;
        public ushort SocketMatchEnchantmentID { get; set; } = 0;
        public ushort TotemCategoryID { get; set; } = 0;
        public ushort InstanceBound { get; set; } = 0;
        public ushort ZoneBound0 { get; set; } = 0;
        public ushort ZoneBound1 { get; set; } = 0;
        public ushort ItemSet { get; set; } = 0;
        public ushort LockID { get; set; } = 0;
        public ushort PageID { get; set; } = 0;
        public ushort ItemDelay { get; set; } = 0;
        public ushort MinFactionID { get; set; } = 0;
        public ushort RequiredSkillRank { get; set; } = 0;
        public ushort RequiredSkill { get; set; } = 0;
        public ushort ItemLevel { get; set; } = 0;
        public short AllowableClass { get; set; } = 0;
        public byte ArtifactID { get; set; } = 0;
        public byte SpellWeight { get; set; } = 0;
        public byte SpellWeightCategory { get; set; } = 0;
        public byte SocketType0 { get; set; } = 0;
        public byte SocketType1 { get; set; } = 0;
        public byte SocketType2 { get; set; } = 0;
        public byte SheatheType { get; set; } = 0;
        [Db2Description("Set this value in Item tab.")]
        public byte Material { get; set; } = 0;
        public byte PageMaterialID { get; set; } = 0;
        public byte Bonding { get; set; } = 0;
        public byte DamageType { get; set; } = 0;
        public sbyte StatModifier_BonusStat0 { get; set; } = 0;
        public sbyte StatModifier_BonusStat1 { get; set; } = 0;
        public sbyte StatModifier_BonusStat2 { get; set; } = 0;
        public sbyte StatModifier_BonusStat3 { get; set; } = 0;
        public sbyte StatModifier_BonusStat4 { get; set; } = 0;
        public sbyte StatModifier_BonusStat5 { get; set; } = 0;
        public sbyte StatModifier_BonusStat6 { get; set; } = 0;
        public sbyte StatModifier_BonusStat7 { get; set; } = 0;
        public sbyte StatModifier_BonusStat8 { get; set; } = 0;
        public sbyte StatModifier_BonusStat9 { get; set; } = 0;
        public byte ContainerSlots { get; set; } = 0;
        public byte RequiredPVPMedal { get; set; } = 0;
        public byte RequiredPVPRank { get; set; } = 0;
        public sbyte RequiredLevel { get; set; } = 0;
        [Db2Description("Set this value in Item tab.")]
        public sbyte InventoryType { get; set; } = 0;
        public sbyte OverallQualityID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
