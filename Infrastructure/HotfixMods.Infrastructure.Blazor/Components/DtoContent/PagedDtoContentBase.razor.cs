using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class PagedDtoContentBase<TDto, TValue> : ComponentBase
        where TDto : IDto
    {
        [CascadingParameter(Name = "Dto")]
        public IDto Dto { get; set; }
        [CascadingParameter(Name = "DtoCompare")]
        public IDto DtoCompare { get; set; }

        public Transition Transition { get; set; }
        public TValue Value { get; set; }
        public TValue ValueCompare { get; set; }

        protected override void OnParametersSet()
        {
            var type = Dto.GetType();
            var property = type.GetProperty(typeof(TValue).Name);
            Value = (TValue)property.GetValue(Dto);
            base.OnParametersSet();
        }
    }
}
