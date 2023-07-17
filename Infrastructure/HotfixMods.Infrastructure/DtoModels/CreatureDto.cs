using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureDto : DtoBase
    {
        public CreatureDto() : base("Creature") { }

        public CreatureTemplate CreatureTemplate { get; set; } = new();
        public CreatureModelInfo CreatureModelInfo { get; set; } = new();
        public CreatureTemplateModel CreatureTemplateModel { get; set; } = new();
        public CreatureDisplayInfo CreatureDisplayInfo { get; set; } = new();
        public CreatureTemplateScaling? CreatureTemplateScaling { get; set; }
        public CreatureDisplayInfoExtra? CreatureDisplayInfoExtra { get; set; }
        public CreatureEquipTemplate? CreatureEquipTemplate { get; set; }
        public CreatureTemplateAddon? CreatureTemplateAddon { get; set; }

        public List<CreatureDisplayInfoOption>? CreatureDisplayInfoOption { get; set; } 
        public List<NpcModelItemSlotDisplayInfo>? NpcModelItemSlotDisplayInfo { get; set; } 

        // TODO: CreatureDisplayInfoGeosetData
    }
}
