using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureEquipTemplate
    {
        public uint CreatureId { get; set; } = 0;
        public byte Id { get; set; } = 0;
        public uint ItemId1 { get; set; } = 0;
        public ushort AppearanceModId1 { get; set; } = 0;
        public ushort ItemVisual1 { get; set; } = 0;
        public uint ItemId2 { get; set; } = 0;
        public ushort AppearanceModId2 { get; set; } = 0;
        public ushort ItemVisual2 { get; set; } = 0;
        public uint ItemId3 { get; set; } = 0;
        public ushort AppearanceModId3 { get; set; } = 0;
        public ushort ItemVisual3 { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
