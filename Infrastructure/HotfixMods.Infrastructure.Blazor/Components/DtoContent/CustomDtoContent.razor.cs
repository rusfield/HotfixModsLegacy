using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class CustomDtoContent : ComponentBase
    {
        [CascadingParameter(Name = "ValueIsNull")]
        public bool ValueIsNull { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
