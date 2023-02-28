using HotfixMods.Infrastructure.Blazor.Pages;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Blazor.Handlers
{
    public static class IconHandler
    {
        public static string GetPageIcon(Type pageType)
        {
            if (pageType == typeof(Creatures))
                return Icons.Material.Filled.FaceRetouchingNatural;
            else if (pageType == typeof(Gameobjects))
                return Icons.Material.Filled.OutlinedFlag;
            else if (pageType == typeof(Items))
                return Icons.Material.Outlined.Shield;
            else if (pageType == typeof(Spells))
                return Icons.Material.Filled.AutoFixHigh;
            else if (pageType == typeof(SpellVisualKits))
                return Icons.Material.Filled.AutoAwesome;
            else if (pageType == typeof(AnimKits))
                return Icons.Material.Filled.Group;
            else if (pageType == typeof(SoundKits))
                return Icons.Material.Filled.VolumeUp;
            else if (pageType == typeof(Settings))
                return Icons.Material.Filled.Settings;
            else
                return Icons.Material.Filled.FormatListBulleted; // Dashboards and fallback
        }
    }
}
