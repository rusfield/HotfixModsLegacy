using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [ClientOnly]
    public class AnimKitPriority
    {
        public uint ID { get; set; }
        public byte Priority { get; set; }
    }
}
