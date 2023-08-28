using HotfixMods.Providers.Models;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class HotfixDto : DtoBase
    {
        public DbRow DbRow { get; set; } = new();
    }
}
