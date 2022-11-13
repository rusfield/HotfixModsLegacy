using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateAddon
    {
        public uint Entry { get; set; } = 0;
        public uint Path_Id { get; set; } = 0;
        public uint Mount { get; set; } = 0;
        public uint MountCreatureId { get; set; } = 0;
        public uint Bytes1 { get; set; } = 0;
        public uint Bytes2 { get; set; } = 0;
        public uint Emote { get; set; } = 0;
        public short AiAnimKit { get; set; } = 0;
        public short MovementAnimKit { get; set; } = 0;
        public short MeleeAnimKit { get; set; } = 0;
        public byte VisibilityDistanceType { get; set; } = 0;
        public string Auras { get; set; } = "";
        // VerifiedBuild missing
    }
}
