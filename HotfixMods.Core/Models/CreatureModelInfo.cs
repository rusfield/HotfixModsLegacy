using HotfixMods.Core.Models.Interfaces;

namespace HotfixMods.Core.Models
{
    public class CreatureModelInfo : IWorldSchema
    {
        public int DisplayId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
