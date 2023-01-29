using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemEffect
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public byte LegacySlotIndex { get; set; } = 0;
        public sbyte TriggerType { get; set; } = 0;
        public short Charges { get; set; } = 0;
        public int CoolDownMSec { get; set; } = 0;
        public int CategoryCoolDownMSec { get; set; } = 0;
        public ushort SpellCategoryId { get; set; } = 0;
        public int SpellId { get; set; } = 0;
        public ushort ChrSpecializationId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
