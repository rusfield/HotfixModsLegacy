using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class NpcModelItemSlotDisplayInfo
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public int ItemDisplayInfoId { get; set; } = 0;
        public sbyte ItemSlot { get; set; } = 0;
        [ParentIndexField]
        public int NpcModelId { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
