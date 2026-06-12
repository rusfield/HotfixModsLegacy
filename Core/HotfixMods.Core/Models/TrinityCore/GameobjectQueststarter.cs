using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class GameobjectQueststarter
    {
        [IndexField]
        public uint Id { get; set; } = 0;
        public uint Quest { get; set; } = 0;
        public uint VerifiedBuild { get; set; } = 0;
    }
}
