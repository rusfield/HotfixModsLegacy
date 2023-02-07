using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class ItemEffect
    {
        [IndexField]
        public uint ID { get; set; } = 0;
        public byte LegacySlotIndex { get; set; } = 0;
        public sbyte TriggerType { get; set; } = 0;
        public short Charges { get; set; } = 0;
        public int CoolDownMSec { get; set; } = 0;
        public int CategoryCoolDownMSec { get; set; } = 0;
        public ushort SpellCategoryID { get; set; } = 0;
        public int SpellID { get; set; } = 0;
        public ushort ChrSpecializationID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
