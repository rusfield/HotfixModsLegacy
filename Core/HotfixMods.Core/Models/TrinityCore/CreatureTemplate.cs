using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplate
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public uint Difficulty_Entry_1 { get; set; } = 0;
        public uint Difficulty_Entry_2 { get; set; } = 0;
        public uint Difficulty_Entry_3 { get; set; } = 0; 
        public uint KillCredit1 { get; set; } = 0;
        public uint KillCredit2 { get; set; } = 0;
        public string Name { get; set; } = "";
        public string FemaleName { get; set; } = "";
        public string Subname { get; set; } = "";
        public string TitleAlt { get; set; } = "";
        public string IconName { get; set; } = "";
        public uint Gossip_Menu_Id { get; set; } = 0;
        public short Minlevel { get; set; } = 1;
        public short Maxlevel { get; set; } = 1;
        public int HealthScalingExpansion { get; set; } = 0;
        public int RequiredExpansion { get; set; } = 0;
        public int VignetteId { get; set; } = 0;
        public ushort Faction { get; set; } = 0;
        public ulong NpcFlag { get; set; } = 0;
        public decimal Speed_Walk { get; set; } = 1;
        public decimal Speed_Run { get; set; } = 1.14286M;
        public decimal Scale { get; set; } = 1;
        public byte Rank { get; set; } = 1;
        public sbyte Dmgschool { get; set; } = 0;
        public uint BaseAttackTime { get; set; } = 2000;
        public uint RangeAttackTime { get; set; } = 2000;
        public decimal BaseVariance { get; set; } = 1;
        public decimal RangeVariance { get; set; } = 1;
        public byte Unit_Class { get; set; } = 1;
        public uint Unit_Flags { get; set; } = 0;
        public uint Unit_Flags2 { get; set; } = 0;
        public uint Unit_Flags3 { get; set; } = 0;
        public uint DynamicFlags { get; set; } = 0;
        public int Family { get; set; } = 0;
        public byte Trainer_Class { get; set; } = 0;
        public byte Type { get; set; } = 0;
        public uint Type_Flags { get; set; } = 0;
        public uint Type_Flags2 { get; set; } = 0;
        public uint LootId { get; set; } = 0;
        public uint PickpocketLoot { get; set; } = 0;
        public uint SkinLoot { get; set; } = 0;
        public uint VehicleId { get; set; } = 0;
        public uint MinGold { get; set; } = 0;
        public uint MaxGold { get; set; } = 0;
        public string AiName { get; set; } = "SmartAI";
        public byte MovementType { get; set; } = 0;
        public decimal HoverHeight { get; set; } = 1;
        public decimal HealthModifier { get; set; } = 1;
        public decimal HealthModifierExtra { get; set; } = 1;
        public decimal ManaModifier { get; set; } = 1;
        public decimal ManaModifierExtra { get; set; } = 1;
        public decimal ArmorModifier { get; set; } = 1;
        public decimal DamageModifier { get; set; } = 1;
        public decimal ExperienceModifier { get; set; } = 1;
        public byte RacialLeader { get; set; } = 0;
        public uint MovementId { get; set; } = 0;
        public int CreatureDifficultyId { get; set; } = 0;
        public int WidgetSetId { get; set; } = 0;
        public int WidgetSetUnitConditionId { get; set; } = 0;
        public byte RegenHealth { get; set; } = 1;
        public ulong Mechanic_Immune_Mask { get; set; } = 0;
        public uint Spell_School_Immune_Mask { get; set; } = 0;
        public uint Flags_Extra { get; set; } = 0;
        public string ScriptName { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
