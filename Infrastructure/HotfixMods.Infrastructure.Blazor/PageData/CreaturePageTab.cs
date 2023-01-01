using HotfixMods.Infrastructure.Blazor.Pages;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.PageData
{
    public class CreaturePageTab : PageTab
    {
        public CreaturePageTab() : base("Creature", typeof(Creatures), typeof(CreatureDto))
        {
        }
    }
}
