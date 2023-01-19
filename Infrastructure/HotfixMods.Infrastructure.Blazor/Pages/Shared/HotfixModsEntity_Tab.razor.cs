using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Blazor.Components.DtoContent;
using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Pages.Shared
{
    public partial class HotfixModsEntity_Tab<TDto> : DtoContentBase<TDto, HotfixModsEntity>
        where TDto : IDto
    {
        [CascadingParameter(Name = "RefreshPage")]
        public Action RefreshPage { get; set; }

        void OnIsUpdateChanged(bool isUpdate)
        {
            PageTab.Dto.IsUpdate = isUpdate;
            RefreshPage.Invoke();
        }

        async Task<Dictionary<bool, string>> GetOptions()
        {
            return new() {
            { true, $"Update existing" },
            { false, "Save new"}
        };
        }
    }
}
