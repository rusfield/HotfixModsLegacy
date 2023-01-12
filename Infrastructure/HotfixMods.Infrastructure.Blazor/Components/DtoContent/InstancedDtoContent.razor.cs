using HotfixMods.Infrastructure.Blazor.PageData;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class InstancedDtoContent : ComponentBase
    {
        [CascadingParameter(Name = "InstanceData")]
        public InstanceData InstanceData { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public Transition Transition { get; set; }

        [Parameter]
        public EventCallback<Transition> TransitionChanged { get; set; }

        bool first = true;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        void NavigateInstance(int newIndex)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                Transition = Transition.Custom;
                TransitionChanged.InvokeAsync(Transition);
            }
        }
    }
}
