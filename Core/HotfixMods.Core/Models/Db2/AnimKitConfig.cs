using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKitConfig
    {
        [IndexField]
        public int ID { get; set; } = 0;
        [Db2Description("Configurations for the current segment.$These can in many cases be left off.")]
        public uint ConfigFlags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
