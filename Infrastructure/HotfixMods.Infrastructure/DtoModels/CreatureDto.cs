using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureDto : DtoBase
    {
        public CreatureDto() : base("Creature") { }

        public CreatureTemplate CreatureTemplate { get; set; } = new();
        public CreatureTemplateAddon? CreatureTemplateAddon { get; set; }
        public CreatureTemplateModel? CreatureTemplateModel { get; set; } = new();
        public CreatureDisplayInfo? CreatureDisplayInfo { get; set; } = new();
        public CreatureDisplayInfoExtra? CreatureDisplayInfoExtra { get; set; }
        public CreatureEquipTemplate? CreatureEquipTemplate { get; set; }
        public CreatureModelInfo? CreatureModelInfo { get; set; } = new();
        public List<CreatureDisplayInfoOption> CreatureDisplayInfoOption { get; set; } 
        public List<NpcModelItemSlotDisplayInfo> NpcModelItemSlotDisplayInfo { get; set; } 
        public int ChrModelId { get; set; } = 0; // Used to help load correct customizations

        // TODO: CreatureDisplayInfoGeosetData
    }
}
