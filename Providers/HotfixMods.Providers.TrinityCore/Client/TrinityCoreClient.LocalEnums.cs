using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Flags.TrinityCore;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using Microsoft.Extensions.Caching.Memory;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient
    {
        Type? GetLocalEnumType(Type? modelType, string propertyName)
        {
            if (typeof(CreatureTemplate) == modelType)
            {
                return propertyName switch
                {
                    nameof(CreatureTemplate.Classification) => typeof(CreatureTemplateRank),
                    nameof(CreatureTemplate.MovementType) => typeof(CreatureTemplateMovementType),
                    nameof(CreatureTemplate.Trainer_Class) => typeof(ChrClassId),
                    nameof(CreatureTemplate.Dmgschool) => typeof(SpellSchool),
                    nameof(CreatureTemplate.NpcFlag) => typeof(CreatureTemplateNpcFlags),
                    nameof(CreatureTemplate.Flags_Extra) => typeof(CreatureTemplateFlagsExtra),
                    nameof(CreatureTemplate.Unit_Flags) => typeof(CreatureTemplateUnitFlags1),
                    nameof(CreatureTemplate.Unit_Flags2) => typeof(CreatureTemplateUnitFlags2),
                    nameof(CreatureTemplate.Unit_Flags3) => typeof(CreatureTemplateUnitFlags3),
                    nameof(CreatureTemplate.Unit_Class) => typeof(CreatureTemplateUnitClass),
                    nameof(CreatureTemplate.RequiredExpansion) => typeof(CreatureTemplateDifficultyRequiredExpansion),
                    _ => null
                };
            }

            if (typeof(Item) == modelType)
            {
                return propertyName switch
                {
                    nameof(Item.SheatheType) => typeof(Item_SheatheTypes),
                    nameof(Item.InventoryType) => typeof(ItemInventoryType),
                    _ => null
                };
            }

            if (typeof(ItemSparse) == modelType)
            {
                return propertyName switch
                {
                    nameof(ItemSparse.StatModifier_BonusStat0) or
                    nameof(ItemSparse.StatModifier_BonusStat1) or
                    nameof(ItemSparse.StatModifier_BonusStat2) or
                    nameof(ItemSparse.StatModifier_BonusStat3) or
                    nameof(ItemSparse.StatModifier_BonusStat4) or
                    nameof(ItemSparse.StatModifier_BonusStat5) or
                    nameof(ItemSparse.StatModifier_BonusStat6) or
                    nameof(ItemSparse.StatModifier_BonusStat7) or
                    nameof(ItemSparse.StatModifier_BonusStat8) or
                    nameof(ItemSparse.StatModifier_BonusStat9) => typeof(ItemModType),
                    nameof(ItemSparse.OverallQualityID) => typeof(ItemQuality),
                    nameof(ItemSparse.Bonding) => typeof(ItemBondingType),
                    nameof(ItemSparse.MinReputation) => typeof(ReputationRank),
                    nameof(ItemSparse.DamageType) => typeof(SpellSchool),
                    _ => null
                };
            }

            if (typeof(ItemEffect) == modelType)
            {
                return propertyName switch
                {
                    nameof(ItemEffect.TriggerType) => typeof(ItemEffectTriggerType),
                    _ => null
                };
            }

            if (typeof(SpellAuraOptions) == modelType)
            {
                return propertyName switch
                {
                    nameof(SpellAuraOptions.DifficultyID) => typeof(MapType),
                    _ => null
                };
            }

            if (typeof(SpellPower) == modelType)
            {
                return propertyName switch
                {
                    nameof(SpellPower.PowerType) => typeof(SpellPowerType),
                    _ => null
                };
            }

            if (typeof(GameobjectTemplate) == modelType)
            {
                return propertyName switch
                {
                    nameof(GameobjectTemplate.Type) => typeof(GameobjectType),
                    _ => null
                };
            }

            if (typeof(GameobjectTemplateAddon) == modelType)
            {
                return propertyName switch
                {
                    nameof(GameobjectTemplateAddon.Flags) => typeof(GameobjectFlags),
                    _ => null
                };
            }

            return propertyName switch
            {
                "EquipmentSlots" => typeof(CharacterInventorySlot),
                _ => null
            };
        }

        Dictionary<TKey, string> GetEnumValuesFromLocalEnum<TKey>(Type enumType)
            where TKey : notnull
        {
            string cacheKey = $"local:{enumType.FullName}:{typeof(TKey).FullName}";
            if (_cache.TryGetValue(cacheKey, out var cachedResults))
                return (Dictionary<TKey, string>)cachedResults;

            var results = new Dictionary<TKey, string>();
            foreach (var value in Enum.GetValues(enumType))
            {
                var key = (TKey)Convert.ChangeType(value, typeof(TKey));
                results[key] = UnderscoreToCase(value.ToString() ?? string.Empty);
            }

            if (CacheResults)
                _cache.Set(cacheKey, results, _cacheOptions);

            return results;
        }
    }
}
