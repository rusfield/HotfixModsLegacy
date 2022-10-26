using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemEffect
    {
        public int Id { get; set; }
        public byte LegacySlotIndex { get; set; }
        public sbyte TriggerType { get; set; }
        public short Charges { get; set; }
        public int CoolDownMSec { get; set; }
        public int CategoryCoolDownMSec { get; set; }
        public ushort SpellCategoryId { get; set; }
        public int SpellId { get; set; }
        public ushort ChrSpecializationId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
