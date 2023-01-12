using HotfixMods.Infrastructure.Blazor.PageData;
using HotfixMods.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class PagedDtoContent<T> : ComponentBase
        where T : class, new()
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public Transition Transition { get; set; }

        [Parameter]
        public EventCallback OnInitValue { get; set; }

        bool valueIsNull = true;
        bool first = true;
        MudCarousel<object>? mudCarouselRef;

        protected override void OnParametersSet()
        {
            valueIsNull = null == PageTab?.Dto?.GetDtoValue<T>();
            base.OnParametersSet();
        }

        void NavigateForward_Click()
        {
            if (mudCarouselRef != null && mudCarouselRef.SelectedIndex < mudCarouselRef.Items.Count)
                mudCarouselRef.MoveTo(mudCarouselRef.SelectedIndex + 1);
        }

        void NavigateBackward_Click()
        {
            if (mudCarouselRef != null && mudCarouselRef.SelectedIndex > 0)
                mudCarouselRef?.MoveTo(mudCarouselRef.SelectedIndex - 1);
        }

        void NavigatePage(int newIndex)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                Transition = Transition.Slide;
            }
        }
    }
}
