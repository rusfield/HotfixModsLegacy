using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class SoundKitDto : BaseDto
    {
        public SoundKitDto() : base(nameof(SoundKit)) { }

        public SoundKit SoundKit { get; set; } = new();
        public List<EntryGroup> EntryGroups { get; set; } = new();

        public class EntryGroup
        {
            public SoundKitEntry SoundKitEntry { get; set; } = new();
        }
    }
}
