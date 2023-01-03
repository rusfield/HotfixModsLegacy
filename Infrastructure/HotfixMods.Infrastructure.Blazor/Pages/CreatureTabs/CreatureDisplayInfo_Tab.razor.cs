using HotfixMods.Core.Models.Db2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Pages.CreatureTabs
{
    public partial class CreatureDisplayInfo_Tab : ComponentBase
    {
        [CascadingParameter(Name = "Transition")]
        public Transition Transition { get; set; }

        CreatureDisplayInfo Value = new();
        CreatureDisplayInfo? ValueCompare = null;
    }
}
