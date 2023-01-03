using HotfixMods.Core.Models.TrinityCore;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Pages.CreatureTabs
{
    public partial class CreatureEquipTemplate_Tab : ComponentBase
    {
        [CascadingParameter(Name = "Transition")]
        public Transition Transition { get; set; }

        CreatureEquipTemplate Value = new();
        CreatureEquipTemplate? ValueCompare = null;
    }
}
