using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class Creature
    {
        public ulong Guid { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
