using HotfixMods.Infrastructure.Blazor.PageData;
using HotfixMods.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class StandardDtoContent<T> : ComponentBase
        where T : class, new()
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        bool valueIsNull = true;

        protected override void OnAfterRender(bool firstRender)
        {
            valueIsNull = null == PageTab?.Dto?.GetDtoValue<T>();
            base.OnAfterRender(firstRender);
        }

        protected override void OnParametersSet()
        {
            valueIsNull = null == PageTab?.Dto?.GetDtoValue<T>();
            base.OnParametersSet();
        }

        void CreateValue()
        {
            PageTab.Dto.SetDtoValueToDefault<T>();
            valueIsNull = false;
        }
    }
}
