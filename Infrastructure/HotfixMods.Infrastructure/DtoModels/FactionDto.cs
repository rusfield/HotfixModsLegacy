using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class FactionDto : DtoBase
    {
        public FactionDto() : base(nameof(Faction)) { }

        public Faction Faction { get; set; } = new();
        public FactionTemplate FactionTemplate { get; set; } = new();
    }
}
