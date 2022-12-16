using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class CreatureDto : DtoBase
    {
        public CreatureDto() : base("Creature") { }

        public CreatureTemplate CreatureTemplate { get; set; } = new();
        public CreatureTemplateAddon? CreatureTemplateAddon { get; set; } = new();
        public CreatureTemplateModel? CreatureTemplateModel { get; set; } = new();
        public CreatureDisplayInfo? CreatureDisplayInfo { get; set; } = new();
        public CreatureDisplayInfoExtra? CreatureDisplayInfoExtra { get; set; } = new();
        public CreatureEquipTemplate? CreatureEquipTemplate { get; set; } = new();
        public CreatureModelInfo? CreatureModelInfo { get; set; } = new();
        public List<CreatureDisplayInfoOption> CreatureDisplayInfoOptions { get; set; } = new();
        public List<NpcModelItemSlotDisplayInfo> NpcModelItemSlotDisplayInfos { get; set; } = new();
    }
}
