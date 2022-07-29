using HotfixMods.Core.Enums;
using HotfixMods.Core.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Constants
{
    public abstract class ItemDefaults
    {
        public const string Name = "New item";
        public const int RequiredLevel = 0;
        public const int ItemLevel = 1;
        public const int IconId = 0;
        public const OverallQualities OverallQuality = OverallQualities.UNCOMMON;
        public const ItemBondings Bonding = ItemBondings.BINDS_WHEN_PICKED_UP;
        public const ItemFlags0 Flags0 = ItemFlags0.DEFAULT;
        public const ItemFlags1 Flags1 = ItemFlags1.DEFAULT;
        public const ItemFlags2 Flags2 = ItemFlags2.DEFAULT;
        public const ItemFlags3 Flags3 = ItemFlags3.DEFAULT;
        public const ItemDisplayInfoFlags DisplayInfoFlag = ItemDisplayInfoFlags.DEFAULT;
        public const ItemMaterial Material = ItemMaterial.NONE;
        public const int ModelMaterialResourcesId = 0;
        public const int ModelResourceId = 0;
        public const ItemRaceFlags AllowableRaces = ItemRaceFlags.ALL;
        public const ItemClassFlags AllowableClasses = ItemClassFlags.ALL;
    }
}
