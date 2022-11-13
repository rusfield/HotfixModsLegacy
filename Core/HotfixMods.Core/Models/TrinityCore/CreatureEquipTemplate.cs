using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureEquipTemplate
    {
        public uint CreatureId { get; set; } = 0;
        public byte Id { get; set; } = 0;
        public uint ItemID1 { get; set; } = 0;
        public ushort AppearanceModID1 { get; set; } = 0;
        public ushort ItemVisual1 { get; set; } = 0;
        public uint ItemID2 { get; set; } = 0;
        public ushort AppearanceModID2 { get; set; } = 0;
        public ushort ItemVisual2 { get; set; } = 0;
        public uint ItemID3 { get; set; } = 0;
        public ushort AppearanceModID3 { get; set; } = 0;
        public ushort ItemVisual3 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
