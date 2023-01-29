using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfig
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public uint ConfigFlags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
