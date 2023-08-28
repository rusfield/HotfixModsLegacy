using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class NpcModelItemSlotDisplayInfo
    {
        
        public int ID { get; set; } = 0;
        public int ItemDisplayInfoID { get; set; } = 0;
        public sbyte ItemSlot { get; set; } = 0;
        
        public int NpcModelID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
