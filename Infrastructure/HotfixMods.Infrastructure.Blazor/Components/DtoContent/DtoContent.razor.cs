using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class DtoContent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
