using HotfixMods.Core.Models;
using HotfixMods.Core.Models.TrinityCore;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class HotfixDto : DtoBase
    {
        public DbRow DbRow { get; set; } = new();
    }
}
