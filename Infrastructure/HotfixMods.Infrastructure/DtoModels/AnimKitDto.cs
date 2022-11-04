using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class AnimKitDto : BaseDto
    {
        public AnimKitDto() : base(nameof(AnimKit)) { }

        public AnimKit AnimKit { get; set; } = new();
        public List<AnimKitSegment> AnimKitSegments { get; set; } = new List<AnimKitSegment>();
    }
}
