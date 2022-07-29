using HotfixMods.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        DisplayTypes GetDisplayType()
        {
            return DisplayTypes.HANDS;
        }

        InventoryTypes GetInventoryType()
        {
            return InventoryTypes.HANDS;
        }

        ItemClass GetClass()
        {
            return ItemClass.ARMOR;
        }

        int GetSubclassId()
        {
            return 5;
        }
    }
}
