using HotfixMods.Core.Models.Db2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Pages.CreatureTabs
{
    public partial class CreatureDisplayInfoExtra_Tab : ComponentBase
    {
        [CascadingParameter(Name = "Transition")]
        public Transition Transition { get; set; }

        CreatureDisplayInfoExtra Value = new();
        CreatureDisplayInfoExtra? ValueCompare = null;
    }
}
