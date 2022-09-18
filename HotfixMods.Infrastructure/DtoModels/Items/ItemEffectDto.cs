using HotfixMods.Core.Enums;

namespace HotfixMods.Infrastructure.DtoModels.Items
{
    public class ItemEffectDto
    {
        public ItemTriggerType? TriggerType { get; set; }
        public int? Charges { get; set; }
        public int? CoolDownMSec { get; set; }
        public int? CategoryCoolDownMSec { get; set; }
        public int? SpellCategoryId { get; set; }
        public int? SpellId { get; set; }
    }
}
