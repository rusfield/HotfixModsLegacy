using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Blazor.Components.DtoContent;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.Pages.Shared
{
    public partial class HotfixModsEntity_Tab<TDto> : DtoContentBase<TDto, HotfixModsEntity>
        where TDto : IDto
    {
        async Task<Dictionary<bool, string>> GetOptions()
        {
            return new() {
            { true, $"{Value.RecordId} (update existing)" },
            { false, "New (generated after save)"}
        };
        }
    }
}
