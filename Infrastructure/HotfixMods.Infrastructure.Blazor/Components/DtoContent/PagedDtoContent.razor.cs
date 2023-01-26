using HotfixMods.Infrastructure.Blazor.PageData;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class PagedDtoContent<T> : ComponentBase
        where T : class, new()
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }

        [CascadingParameter(Name = "InstanceData")]
        public InstanceData InstanceData { get; set; }

        [CascadingParameter(Name = "GroupIndex")]
        public int GroupIndex { get; set; }
        [CascadingParameter(Name = "ValueIsNull")]
        public bool ValueIsNull { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public Transition Transition { get; set; }

        [Parameter]
        public bool IsCustom { get; set; } = false;

        bool first = true;
        MudCarousel<object>? mudCarouselRef;
        bool nextPageDisabled = true;
        bool previousPageDisabled = true;

        protected override void OnAfterRender(bool firstRender)
        {
            // Re-render if things changes.
            // Without this, the navigation arrows would "fall behind". 
            var nextPageDisabledChanged = nextPageDisabled;
            var previousPageDisabledChanged = previousPageDisabled;

            nextPageDisabled = mudCarouselRef == null || mudCarouselRef.SelectedIndex >= mudCarouselRef.Items.Count - 1;
            previousPageDisabled = mudCarouselRef == null || mudCarouselRef.SelectedIndex <= 0;
            if (nextPageDisabledChanged != nextPageDisabled || previousPageDisabledChanged != previousPageDisabled)
                StateHasChanged();
            base.OnAfterRender(firstRender);
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
