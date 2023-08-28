using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateAddon
    {
        
        public uint Entry { get; set; } = 0;
        public uint Path_ID { get; set; } = 0;
        public uint Mount { get; set; } = 0;
        public uint MountCreatureID { get; set; } = 0;
        public byte StandState { get; set; } = 0;
        public byte AnimTier { get; set; } = 0;
        public byte VisFlags { get; set; } = 0;
        public byte SheathState { get; set; } = 0;
        public byte PvPFlags { get; set; } = 0;
        public uint Emote { get; set; } = 0;
        public short AiAnimKit { get; set; } = 0;
        public short MovementAnimKit { get; set; } = 0;
        public short MeleeAnimKit { get; set; } = 0;
        public byte VisibilityDistanceType { get; set; } = 0;
        public string Auras { get; set; } = "";
        // VerifiedBuild missing
    }
}
