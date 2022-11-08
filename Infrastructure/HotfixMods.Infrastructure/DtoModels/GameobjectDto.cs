using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class GameobjectDto : BaseDto
    {
        public GameobjectDto(string displayName) : base(displayName){}

        public GameobjectTemplate GameobjectTemplate { get; set; } = new();
        public GameobjectTemplateAddon GameobjectTemplateAddon { get; set; } = new();
        public GameobjectDisplayInfo GameobjectDisplayInfo { get; set; } = new();
    }
}
