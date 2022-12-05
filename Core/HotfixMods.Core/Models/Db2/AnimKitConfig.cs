using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfig
    {
        public int Id { get; set; } = 0;
        public uint ConfigFlags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
