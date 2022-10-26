using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class NpcModelItemSlotDisplayInfo
    {
        public int Id { get; set; }
        public int ItemDisplayInfoId { get; set; }
        public sbyte ItemSlot { get; set; }
        public int NpcModelId { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
