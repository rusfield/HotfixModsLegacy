using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class PagedDtoContentBase<TDto, TValue> : ComponentBase
        where TDto : IDto
        where TValue : class
    {
        [CascadingParameter(Name = "Dto")]
        public IDto Dto { get; set; }
        [CascadingParameter(Name = "DtoCompare")]
        public IDto DtoCompare { get; set; }

        public Transition Transition { get; set; }
        public TValue Value { get; set; }
        public TValue? ValueCompare { get; set; }

        protected override void OnParametersSet()
        {
            var dtoType = Dto.GetType();
            var dtoProperty = dtoType.GetProperty(typeof(TValue).Name);
            Value = (TValue)dtoProperty.GetValue(Dto);
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if(DtoCompare != null)
            {
                var dtoCompareType = DtoCompare.GetType();
                var dtoCompareProperty = dtoCompareType.GetProperty(typeof(TValue).Name);
                ValueCompare = (TValue)dtoCompareProperty.GetValue(DtoCompare);
            }
            else
            {
                ValueCompare = null;
            }
            base.OnAfterRender(firstRender);
        }
    }
}
