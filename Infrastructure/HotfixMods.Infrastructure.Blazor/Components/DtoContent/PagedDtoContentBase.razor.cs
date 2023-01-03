using HotfixMods.Infrastructure.DtoModels;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class PagedDtoContentBase<TDto, TValue> : DtoContentBase<TDto, TValue>
        where TDto : IDto
        where TValue : class
    {
        public Transition Transition { get; set; }

    }
}
