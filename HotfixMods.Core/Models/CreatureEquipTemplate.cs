using HotfixMods.Core.Models.Interfaces;

namespace HotfixMods.Core.Models
{
    public class CreatureEquipTemplate : IWorldSchema
    {
        public int CreatureId { get; set; }
        public int Id { get; set; }
        public int ItemId1 { get; set; }
        public int AppearanceModId1 { get; set; }
        public int ItemVisual1 { get; set; }
        public int ItemId2 { get; set; }
        public int AppearanceModId2 { get; set; }
        public int ItemVisual2 { get; set; }
        public int ItemId3 { get; set; }
        public int AppearanceModId3 { get; set; }
        public int ItemVisual3 { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
