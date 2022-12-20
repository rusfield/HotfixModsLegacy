using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class Creature
    {
        public int Guid { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
