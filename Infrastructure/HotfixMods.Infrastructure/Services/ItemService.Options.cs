using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;
using HotfixMods.Providers.Listfile;

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
            // Enum exists in TC, but it is currently wrong.
            // return await GetEnumOptionsAsync<byte>(typeof(Item), nameof(Item.SheatheType));
            return Enum.GetValues<Item_SheatheTypes>().ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<int, string>> GetModifiedCraftingReagentItemIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("ModifiedCraftingReagentItem", "Description");
        }

        public async Task<Dictionary<int, string>> GetIconFileDataIdOptionsAsync()
        {
            return await GetIconOptionsAsync<int>();
        }

        public async Task<Dictionary<byte, string>> GetItemClassOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var itemClasses = await GetAsync(_appConfig.HotfixesSchema, "ItemClass");
            foreach (var itemClass in itemClasses.Rows)
            {
                results[itemClass.GetValueByNameAs<byte>("ClassID")] = itemClass.GetValueByNameAs<string>("ClassName");
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemSubClassOptionsAsync(sbyte classId)
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var subClasses = await GetAsync(_appConfig.HotfixesSchema, "ItemSubClass");
            foreach (var subClass in subClasses.Rows)
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

            var materials = await GetAsync(_appConfig.HotfixesSchema, "Material");
            foreach (var material in materials.Rows)
            {
                var key = material.GetValueByNameAs<byte>("ID");
                var value = "";
                if (Enum.IsDefined(typeof(Item_Material), key))
                    value += $"{((Item_Material)key).ToDisplayString()}";

                results[key] = value;
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemGroupSoundsIdOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results.InitializeDefaultValue();

            var itemGroupSounds = await GetAsync(_appConfig.HotfixesSchema, "ItemGroupSounds");
            foreach (var itemGroupSound in itemGroupSounds.Rows)
            {
                var key = itemGroupSound.GetValueByNameAs<byte>("ID");
                var value = "";
                if (Enum.IsDefined(typeof(Item_ItemGroupSoundsId), key))
                    value += $"{((Item_ItemGroupSoundsId)key).ToDisplayString()}";

                results[key] = value;
            }
            return results;
        }

        public async Task<Dictionary<int, string>> GetCraftingQualityIdOptionsAsync()
        {
            var results = new Dictionary<int, string>();
            results.InitializeDefaultValue();

            var craftingQualities = await GetAsync(_appConfig.HotfixesSchema, "CraftingQuality");
            foreach (var craftingQuality in craftingQualities.Rows)
            {
                var key = craftingQuality.GetValueByNameAs<int>("ID");
                var value = "";
                if (Enum.IsDefined(typeof(Item_CraftingQualityId), key))
                    value += $"{((Item_CraftingQualityId)key).ToDisplayString()}";

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
            return await GetDb2OptionsAsync<ushort>("ItemNameDescription", "Description");
        }

        public async Task<Dictionary<ushort, string>> GetItemSetOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("ItemSet", "Name");
        }

        public async Task<Dictionary<long, string>> GetAllowableRaceOptionsAsync()
        {
            var races = Enum.GetValues<ItemSparse_AllowableRace>().ToList();
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ANY_HORDE_RACE), 0);
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ANY_ALLIANCE_RACE), 0);
            races.MoveElement(races.IndexOf(ItemSparse_AllowableRace.ALL), 0);
            return races.ToDictionary(key => (long)key, value => $"{value.ToDisplayString()}");
        }

        public async Task<List<long>> GetExclusiveAllowableRaceOptionsAsync()
        {
            return new() { (long)ItemSparse_AllowableRace.ALL, (long)ItemSparse_AllowableRace.ANY_ALLIANCE_RACE, (long)ItemSparse_AllowableRace.ANY_HORDE_RACE };
        }

        public async Task<Dictionary<short, string>> GetAllowableClassOptionsAsync()
        {
            var classes = Enum.GetValues<ItemSparse_AllowableClass>().ToList();
            classes.MoveElement(classes.IndexOf(ItemSparse_AllowableClass.ALL), 0);
            return classes.ToDictionary(key => (short)key, value => $"{value.ToDisplayString()}");
        }

        public async Task<List<short>> GetExclusiveAllowableClassOptionsAsync()
        {
            return new() { (short)ItemSparse_AllowableClass.ALL };
        }

        public async Task<Dictionary<ushort, string>> GetRequiredHolidayOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("HolidayNames", "Name");
        }

        public async Task<Dictionary<ushort, string>> GetRequiredTransmogHolidayOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("HolidayNames", "Name");
        }

        public async Task<Dictionary<ushort, string>> GetInstanceBoundOptionsAsync()
        {
            // TODO: Default is 0, which actually points at a map.
            // Check whether only maps with InstanceType == Instance are valid, and if so, filter out others and set 0 as None.
            return await GetDb2OptionsAsync<ushort>("Map", "MapName");
        }

        public async Task<Dictionary<ushort, string>> GetZoneBoundOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("AreaTable", "AreaName");
        }

        public async Task<Dictionary<ushort, string>> GetRequiredSkillOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("SkillLine", "DisplayName");
        }

        public async Task<Dictionary<ushort, string>> GetMinFactionIdOptionsAsync()
        {
            return await GetFactionOptionsAsync<ushort>();
        }

        public async Task<Dictionary<int, string>> GetMinReputationOptionsAsync()
        {
            return await GetEnumOptionsAsync<int>(typeof(ItemSparse), nameof(ItemSparse.MinReputation));
        }

        public async Task<Dictionary<int, string>> GetLimitCategoryOptionsAsync()
        {
            var results = new Dictionary<int, string>();
            var categories = await GetAsync(_appConfig.HotfixesSchema, "ItemLimitCategory");
            results[0] = "None";
            foreach(var category in categories.Rows)
            {
                results.Add((int)category.GetIdColumnValue(), $"{category.GetValueByNameAs<string>("Name")} ({category.GetValueByNameAs<string>("Quantity")})");
            }
            return results;
        }

        public async Task<Dictionary<uint, string>> GetRequiredAbilityOptionsAsync()
        {
            return await GetDb2OptionsAsync<uint>("SpellName", "Name");
        }

        public async Task<Dictionary<ushort, string>> GetSocketMatchEnchantmentIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("SpellItemEnchantment", "Name");
        }

        public async Task<Dictionary<int, string>> GetStartQuestIdOptionsAsync()
        {
            // TODO: Check if this can be retrieved from TC tables
            return new();
        }

        // TODO: Look for a better source
        public async Task<Dictionary<byte, string>> GetDamageTypeOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            var schoolOptions = await GetEnumOptionsAsync<byte>(typeof(ItemSparse), nameof(ItemSparse.DamageType));
            for(byte i = 0; i < schoolOptions.Count; i++)
            {
                results[i] = schoolOptions.ElementAt(i).Value;
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetArtifactIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<byte>("Artifact", "Name");
        }

        public async Task<Dictionary<uint, string>> GetBagFamilyOptionsAsync()
        {
            return await GetDb2OptionsAsync<uint>("ItemBagFamily", "Name", true);
        }

        public async Task<Dictionary<int, string>> GetLanguageIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("Languages", "Name");
        }
        #endregion

        #region ItemEffect
        public async Task<Dictionary<sbyte, string>> GetTriggerTypeOptionsAsync()
        {
            return await GetEnumOptionsAsync<sbyte>(typeof(ItemEffect), nameof(ItemEffect.TriggerType));
        }

        public async Task<Dictionary<ushort, string>> GetSpellCategoryIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("SpellCategory", "Name");
        }

        public async Task<Dictionary<ushort, string>> GetChrSpecializationIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("ChrCustomization", "Name");
        }

        public async Task<Dictionary<int, string>> GetSpellIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("SpellName", "Name");
        }
        #endregion

        #region ItemDisplayInfoMaterialRes

        public async Task<Dictionary<int, string>> GetMaterialResourceIdOptionsAsync()
        {
            return await GetTextureFileDataOptionsAsync<int>();
        }

        #endregion

        #region ItemDisplayInfo
        public async Task<Dictionary<int, string>> GetModelMaterialResourcesIdOptionsAsync()
        {
            return await GetTextureFileDataOptionsAsync<int>();
        }

        public async Task<Dictionary<uint, string>> GetModelResourcesIdOptionsAsync()
        {
            return await GetModelFileDataOptionsAsync<uint>();
        }
        #endregion

        #region ItemAppearance
        public async Task<Dictionary<int, string>> GetDisplayTypeOptionsAsync()
        {
            return Enum.GetValues<ItemAppearance_DisplayType>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<int, string>> GetTransmogPlayerConditionIdOptionsAsync()
        {
            return await GetPlayerConditionOptionsAsync<int>();
        }

        public async Task<Dictionary<int, string>> GetParticleColorIdOptionsAsync()
        {
            return await GetParticleColorOptionsAsync<int>();
        }
        #endregion
    }
}
