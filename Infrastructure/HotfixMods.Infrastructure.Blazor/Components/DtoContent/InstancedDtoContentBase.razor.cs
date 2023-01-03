using HotfixMods.Infrastructure.Blazor.PageData;
using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class InstancedDtoContentBase<TDto, TValue> : ComponentBase
        where TDto : IDto
        where TValue : class
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }

        public Transition Transition { get; set; }
        public List<TValue> Values { get; set; }
        public List<TValue>? ValueCompares { get; set; }

        protected override void OnParametersSet()
        {
            var dtoType = PageTab.Dto.GetType();
            var dtoProperty = dtoType.GetProperty(typeof(TValue).Name);
            Values = (List<TValue>)dtoProperty.GetValue(PageTab.Dto);
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (PageTab.DtoCompare != null)
            {
                var dtoCompareType = PageTab.DtoCompare.GetType();
                var dtoCompareProperty = dtoCompareType.GetProperty(typeof(TValue).Name);
                Values = (List<TValue>)dtoCompareProperty.GetValue(PageTab.DtoCompare);
            }
            else
            {
                ValueCompares = null;
            }
            base.OnAfterRender(firstRender);
        }
    }
}
