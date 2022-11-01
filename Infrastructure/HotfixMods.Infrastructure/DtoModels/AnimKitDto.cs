using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class AnimKitDto : BaseDto
    {
        public AnimKit AnimKit { get; set; } = new();
        public List<AnimKitSegment> AnimKitSegments { get; set; } = new List<AnimKitSegment>();
    }
}
