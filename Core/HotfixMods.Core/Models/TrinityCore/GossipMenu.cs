using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class GossipMenu
    {
        [IndexField]
        public uint MenuID { get; set; } = 0;
        public uint TextID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
