using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ItemService
    {
        #region Item
        public async Task<Dictionary<sbyte, string>> GetInventoryTypeOptionsAsync()
        {
            return await GetEnumOptionsAsync<sbyte>(typeof(Item), nameof(Item.InventoryType));
        }
        public async Task<Dictionary<byte, string>> GetSheatheTypeOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(Item), nameof(Item.SheatheType));
        }

        public async Task<Dictionary<int, string>> GetModifiedCraftingReagentItemIdOptionsAsync()
        {
            return await GetClientOptionsAsync<int, uint>(_appConfig.HotfixesSchema, "ModifiedCraftingReagentItem", "Description");
        }

        public async Task<Dictionary<int, string>> GetIconFileDataIdOptionsAsync()
        {
            return await _listfileProvider.GetIconsAsync<int>();
        }

        public async Task<Dictionary<byte, string>> GetItemClassOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var itemClasses = await GetAsync(_appConfig.HotfixesSchema, "ItemClass", false, true);
            foreach (var itemClass in itemClasses)
            {
                results[itemClass.GetValueByNameAs<byte>("ClassID")] = itemClass.GetValueByNameAs<string>("ClassName");
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemSubClassOptionsAsync(sbyte classId)
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var subClasses = await GetAsync(_appConfig.HotfixesSchema, "ItemSubClass", false, true);
            foreach (var subClass in subClasses)
            {
                var classIdOfSubClass = subClass.GetValueByNameAs<sbyte>("ClassID");

                if (classId == classIdOfSubClass)
                {
                    var subClassId = subClass.GetValueByNameAs<byte>("SubClassID");
                    var displayName = subClass.GetValueByNameAs<string>("DisplayName");
                    var verboseName = subClass.GetValueByNameAs<string>("VerboseName");

                    string text = string.IsNullOrWhiteSpace(verboseName) ? displayName : verboseName;

                    results[subClassId] = text;
                }
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemMaterialOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var materials = await GetAsync(_appConfig.HotfixesSchema, "Material", false, true);
            foreach (var material in materials)
            {
                var key = material.GetValueByNameAs<byte>("ID");
                var value = key.ToString();
                if (Enum.IsDefined(typeof(Item_Material), key))
                    value += $" - {((Item_Material)key).ToDisplayString()}";

                results[key] = value;
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemGroupSoundsIdOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var itemGroupSounds = await GetAsync(_appConfig.HotfixesSchema, "ItemGroupSounds", false, true);
            foreach (var itemGroupSound in itemGroupSounds)
            {
                var key = itemGroupSound.GetValueByNameAs<byte>("ID");
                var value = key.ToString();
                if (Enum.IsDefined(typeof(Item_ItemGroupSoundsId), key))
                    value += $" - {((Item_ItemGroupSoundsId)key).ToDisplayString()}";

                results[key] = value;
            }
            return results;
        }

        public async Task<Dictionary<int, string>> GetCraftingQualityIdOptionsAsync()
        {
            var results = new Dictionary<int, string>();
            results.InitializeDefaultValue();

            var craftingQualities = await GetAsync(_appConfig.HotfixesSchema, "CraftingQuality", false, true);
            foreach (var craftingQuality in craftingQualities)
            {
                var key = craftingQuality.GetValueByNameAs<int>("ID");
                var value = key.ToString();
                if (Enum.IsDefined(typeof(Item_CraftingQualityId), key))
                    value += $" - {((Item_CraftingQualityId)key).ToDisplayString()}";

                results[key] = value;
            }
            return results;
        }
        #endregion

        #region ItemSparse
        public async Task<Dictionary<sbyte, string>> GetStatModifierBonusStatOptionsAsync()
        {
            // All returns the same, 0-9
            return await GetEnumOptionsAsync<sbyte>(typeof(ItemSparse), nameof(ItemSparse.StatModifier_BonusStat0));
        }

        public async Task<Dictionary<sbyte, string>> GetOverallQualityIdOptionsAsync()
        {
            return await GetEnumOptionsAsync<sbyte>(typeof(ItemSparse), nameof(ItemSparse.OverallQualityID));
        }

        public async Task<Dictionary<byte, string>> GetBondingOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(ItemSparse), nameof(ItemSparse.Bonding));
        }

        public async Task<Dictionary<ushort, string>> GetItemNameDescriptionIdOptionsAsync()
        {
            return await GetClientOptionsAsync<ushort, uint>(_appConfig.HotfixesSchema, "ItemNameDescription", "Description");
        }

        public async Task<Dictionary<ushort, string>> GetItemSetOptionsAsync()
        {
            return await GetClientOptionsAsync<ushort, uint>(_appConfig.HotfixesSchema, "ItemSet", "Name");
        }

        public async Task<Dictionary<long, string>> GetAllowableRaceOptionsAsync()
        {
            var races = Enum.GetValues<ItemSparse_AllowableRace>().ToList();
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ANY_HORDE_RACE), 0);
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ANY_ALLIANCE_RACE), 0);
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ALL), 0);
            return races.ToDictionary(key => (long)key, value => value.ToDisplayString());
        }

        public async Task<List<long>> GetExclusiveAllowableRaceOptionsAsync()
        {
            return new() { (long)ItemSparse_AllowableRace.ALL, (long)ItemSparse_AllowableRace.ANY_ALLIANCE_RACE, (long)ItemSparse_AllowableRace.ANY_HORDE_RACE };
        }

        public async Task<Dictionary<short, string>> GetAllowableClassOptionsAsync()
        {
            var classes = Enum.GetValues<ItemSparse_AllowableClass>().ToList();
            classes.MoveElement(classes.IndexOf(ItemSparse_AllowableClass.ALL), 0);
            return classes.ToDictionary(key => (short)key, value => value.ToDisplayString());
        }

        public async Task<List<short>> GetExclusiveAllowableClassOptionsAsync()
        {
            return new() { (short)ItemSparse_AllowableClass.ALL };
        }
        #endregion
    }
}
