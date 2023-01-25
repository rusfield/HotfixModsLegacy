using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{

    [WorldSchema]
    public class GameobjectTemplateAddon
    {
        [IndexField]
        public uint Entry { get; set; } = 0;
        public ushort Faction { get; set; } = 0;
        public uint Flags { get; set; } = 0;
        public uint Mingold { get; set; } = 0;
        public uint Maxgold { get; set; } = 0;
        public int Artkit0 { get; set; } = 0;
        public int Artkit1 { get; set; } = 0;
        public int Artkit2 { get; set; } = 0;
        public int Artkit3 { get; set; } = 0;
        public int Artkit4 { get; set; } = 0;
        public uint WorldEffectId { get; set; } = 0;
        public uint AiAnimKitId { get; set; } = 0;

        //public int VerifiedBuild { get; set; } // Currently not implemented in TC 
    }
}
