using HotfixMods.Core.Enums.Db2;
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
        public async Task<Dictionary<byte, string>> GetItemClassOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            var itemClasses = await GetAsync(_appConfig.HotfixesSchema, "ItemClass", false, true);
            foreach (var itemClass in itemClasses)
            {
                results.Add(itemClass.GetValueByNameAs<byte>("ClassID"), itemClass.GetValueByNameAs<string>("ClassName"));
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemSubClassOptionsAsync(sbyte classId)
        {
            var results = new Dictionary<byte, string>();
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

                    results.Add(subClassId, text);
                }
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetItemMaterialOptionsAsync()
        {
            var results = new Dictionary<byte, string>();
            results[0] = "0 - None";
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
        #endregion

        #region ItemSparse
        public async Task<Dictionary<sbyte, string>> GetStatModifierBonusStatOptionsAsync()
        {
            // All returns the same, 0-9
            return await GetEnumOptionsAsync<sbyte>(typeof(ItemSparse), nameof(ItemSparse.StatModifier_BonusStat0));
        }
        #endregion
    }
}
