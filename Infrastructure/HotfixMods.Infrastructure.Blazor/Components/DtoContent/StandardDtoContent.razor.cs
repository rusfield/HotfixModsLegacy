using HotfixMods.Infrastructure.Blazor.PageData;
using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class StandardDtoContent<T> : ComponentBase
        where T : class, new()
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }
        [CascadingParameter(Name = "ValueIsNull")]
        public bool ValueIsNull { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
