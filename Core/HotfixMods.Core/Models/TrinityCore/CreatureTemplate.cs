using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplate
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public uint KillCredit1 { get; set; } = 0;
        public uint KillCredit2 { get; set; } = 0;
        [Db2Description("The name of the creature.")]
        public string Name { get; set; } = "";
        public string FemaleName { get; set; } = "";
        public string Subname { get; set; } = "";
        public string TitleAlt { get; set; } = "";
        public string IconName { get; set; } = "";
        public int RequiredExpansion { get; set; } = 0;
        public int VignetteID { get; set; } = 0;
        public ushort Faction { get; set; } = 7; // Value: Creature
        public ulong NpcFlag { get; set; } = 0;
        public decimal Speed_Walk { get; set; } = 1;
        public decimal Speed_Run { get; set; } = 1.14286M;
        public decimal Scale { get; set; } = 1;
        public byte Classification { get; set; } = 0; 
        public sbyte Dmgschool { get; set; } = 0;
        public uint BaseAttackTime { get; set; } = 2000;
        public uint RangeAttackTime { get; set; } = 2000;
        public decimal BaseVariance { get; set; } = 1;
        public decimal RangeVariance { get; set; } = 1;
        public byte Unit_Class { get; set; } = 1;
        public uint Unit_Flags { get; set; } = 0;
        public uint Unit_Flags2 { get; set; } = 0;
        public uint Unit_Flags3 { get; set; } = 0;
        public int Family { get; set; } = 0;
        public byte Trainer_Class { get; set; } = 0;
        public byte Type { get; set; } = 10; // "Not Specified"
        public uint VehicleID { get; set; } = 0;
        public string AiName { get; set; } = "SmartAI";
        public byte MovementType { get; set; } = 0;
        public decimal ExperienceModifier { get; set; } = 1;
        public byte RacialLeader { get; set; } = 0;
        public uint MovementID { get; set; } = 0;
        public int WidgetSetID { get; set; } = 0;
        public int WidgetSetUnitConditionID { get; set; } = 0;
        public byte RegenHealth { get; set; } = 1;
        public int CreatureImmunitiesID { get; set; } = 0; 
        public uint Flags_Extra { get; set; } = 0;
        public string ScriptName { get; set; } = "";
        public string StringID { get; set; } = "";
        public int VerifiedBuild { get; set; } = -1;
    }

}
