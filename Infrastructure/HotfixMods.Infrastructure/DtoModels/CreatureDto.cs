using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureDto : DtoBase
    {
        public CreatureDto() : base("Creature") { }

        public CreatureTemplate CreatureTemplate { get; set; } = new();
        public CreatureTemplateAddon? CreatureTemplateAddon { get; set; }
        public CreatureTemplateModel? CreatureTemplateModel { get; set; }
        public CreatureDisplayInfo? CreatureDisplayInfo { get; set; }
        public CreatureDisplayInfoExtra? CreatureDisplayInfoExtra { get; set; }
        public CreatureEquipTemplate? CreatureEquipTemplate { get; set; }
        public CreatureModelInfo? CreatureModelInfo { get; set; }
        public List<CreatureDisplayInfoOption> CreatureDisplayInfoOptions { get; set; } = new();
        public List<NpcModelItemSlotDisplayInfo> NpcModelItemSlotDisplayInfos { get; set; } = new();
        public int ChrModelId { get; set; } = 0; // Used to help load correct customizations

        // TODO: CreatureDisplayInfoGeosetData
    }
}
