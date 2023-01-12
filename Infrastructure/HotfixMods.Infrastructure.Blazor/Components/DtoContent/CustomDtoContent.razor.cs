using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class CustomDtoContent : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
