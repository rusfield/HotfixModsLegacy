using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateDifficulty
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public int DifficultyId { get; set; } = 0;
        public short LevelScalingDeltaMin { get; set; } = 0;
        public short LevelScalingDeltaMax { get; set; } = 0;
        public int ContentTuningId { get; set; } = 0;
        public int HealthScalingExpansion { get; set; } = 0; 
        public decimal HealthModifier { get; set; } = 1;
        public decimal ManaModifier { get; set; } = 1;
        public decimal ArmorModifier { get; set; } = 1;
        public decimal DamageModifier { get; set; } = 1; 
        public int CreatureDifficultyID { get; set; } = 0; 
        public uint TypeFlags { get; set; } = 0; 
        public uint TypeFlags2 { get; set; } = 0; 
        public uint TypeFlags3 { get; set; } = 0; 
        public uint LootID { get; set; } = 0; 
        public uint PickPocketLootID { get; set; } = 0; 
        public uint SkinLootID { get; set; } = 0; 
        public uint GoldMin { get; set; } = 0; 
        public uint GoldMax { get; set; } = 0; 
        public uint StaticFlags1 { get; set; } = 0; 
        public uint StaticFlags2 { get; set; } = 0; 
        public uint StaticFlags3 { get; set; } = 0; 
        public uint StaticFlags4 { get; set; } = 0; 
        public uint StaticFlags5 { get; set; } = 0; 
        public uint StaticFlags6 { get; set; } = 0; 
        public uint StaticFlags7 { get; set; } = 0; 
        public uint StaticFlags8 { get; set; } = 0; 
        public int VerifiedBuild { get; set; } = -1;
    }
}
