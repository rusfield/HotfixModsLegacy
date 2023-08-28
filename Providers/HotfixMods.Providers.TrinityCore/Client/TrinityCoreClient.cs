using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Providers.Interfaces;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient : IServerValuesProvider
    {
        public TrinityCoreClient(string trinityCorePath)
        {
            TrinityCorePath = trinityCorePath;
        }

        public string TrinityCorePath { get; set; } = "/";

        public async Task<Dictionary<TKey, string>> GetServerValuesAsync<TKey>(Type? modelType, string propertyName)
            where TKey : notnull
        {
            if (typeof(CreatureTemplate) == modelType)
            {
                return propertyName switch
                {
                    nameof(CreatureTemplate.Rank) => await GetEnumAsync<TKey>(sharedDefines_path, "CreatureEliteType", "CREATURE_ELITE_", "CREATURE_"),
                    nameof(CreatureTemplate.MovementType) => await GetEnumAsync<TKey>(movement_path, "MovementGeneratorType", "_MOTION_TYPE"),
                    nameof(CreatureTemplate.Trainer_Class) => await GetEnumAsync<TKey>(trainer_path, "Type"),
                    nameof(CreatureTemplate.Type_Flags) => await GetEnumAsync<TKey>(sharedDefines_path, "CreatureTypeFlags", "CREATURE_TYPE_FLAG_"),
                    nameof(CreatureTemplate.Type_Flags2) => await GetEnumAsync<TKey>(sharedDefines_path, "CreatureTypeFlags2", "CREATURE_TYPE_FLAG_2_"),
                    nameof(CreatureTemplate.Dmgschool) => await GetEnumAsync<TKey>(sharedDefines_path, "SpellSchools", "SPELL_SCHOOL_"),
                    nameof(CreatureTemplate.Spell_School_Immune_Mask) => await GetEnumAsync<TKey>(sharedDefines_path, "SpellSchoolMask", "SPELL_SCHOOL_MASK_"),
                    nameof(CreatureTemplate.DynamicFlags) => await GetEnumAsync<TKey>(sharedDefines_path, "UnitDynFlags", "UNIT_DYNFLAG_"),
                    nameof(CreatureTemplate.NpcFlag) => await GetEnumAsync<TKey>(unitDefines_path, "NPCFlags", "UNIT_NPC_FLAG_"),
                    nameof(CreatureTemplate.Flags_Extra) => await GetEnumAsync<TKey>(creatureData_path, "CreatureFlagsExtra", "CREATURE_FLAG_EXTRA_"),
                    nameof(CreatureTemplate.Unit_Flags) => await GetEnumAsync<TKey>(unitDefines_path, "UnitFlags", "UNIT_FLAG_"),
                    nameof(CreatureTemplate.Unit_Flags2) => await GetEnumAsync<TKey>(unitDefines_path, "UnitFlags2", "UNIT_FLAG2_"),
                    nameof(CreatureTemplate.Unit_Flags3) => await GetEnumAsync<TKey>(unitDefines_path, "UnitFlags3", "UNIT_FLAG3_"),
                    nameof(CreatureTemplate.Unit_Class) => await GetEnumAsync<TKey>(sharedDefines_path, "Classes", "CLASS_"),
                    nameof(CreatureTemplate.HealthScalingExpansion) or
                    nameof(CreatureTemplate.RequiredExpansion) => await GetEnumAsync<TKey>(sharedDefines_path, "Expansions", "EXPANSION_", "LEVEL_CURRENT"),


                    _ => new()
                };

            }
            else if (typeof(Item) == modelType)
            {
                return propertyName switch
                {
                    nameof(Item.SheatheType) => await GetEnumAsync<TKey>(sharedDefines_path, "SheathTypes", "SHEATHETYPE_"),
                    nameof(Item.InventoryType) => await GetEnumAsync<TKey>(itemTemplate_path, "InventoryType", "INVTYPE_"),
                    _ => new()
                };
            }
            else if (typeof(ItemSparse) == modelType)
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
                    nameof(ItemSparse.StatModifier_BonusStat9) => await GetEnumAsync<TKey>(itemTemplate_path, "ItemModType", "ITEM_MOD_"),
                    nameof(ItemSparse.OverallQualityID) => await GetEnumAsync<TKey>(sharedDefines_path, "ItemQualities", "ITEM_QUALITY_"),
                    nameof(ItemSparse.Bonding) => await GetEnumAsync<TKey>(itemTemplate_path, "ItemBondingType"),
                    nameof(ItemSparse.MinReputation) => await GetEnumAsync<TKey>(sharedDefines_path, "ReputationRank", "REP_"),
                    nameof(ItemSparse.DamageType) => await GetEnumAsync<TKey>(sharedDefines_path, "SpellSchools", "SPELL_SCHOOL_"),

                    _ => new()
                };
            }
            else if (typeof(ItemEffect) == modelType)
            {
                return propertyName switch
                {
                    nameof(ItemEffect.TriggerType) => await GetEnumAsync<TKey>(itemTemplate_path, "ItemSpellTriggerType", "ITEM_SPELLTRIGGER_"),

                    _ => new()
                };
            }
            else if (typeof(SpellAuraOptions) == modelType)
            {
                return propertyName switch
                {
                    nameof(SpellAuraOptions.DifficultyID) => await GetEnumAsync<TKey>(dbcEnums_path, "MapTypes", "MAP_"),

                    _ => new()
                };
            }
            else if (typeof(SpellPower) == modelType)
            {
                return propertyName switch
                {
                    nameof(SpellPower.PowerType) => await GetEnumAsync<TKey>(sharedDefines_path, "Powers", "POWER_"),

                    _ => new()
                };
            }
            else if (typeof(GameobjectTemplate) == modelType)
            {
                return propertyName switch
                {
                    nameof(GameobjectTemplate.Type) => await GetEnumAsync<TKey>(sharedDefines_path, "GameobjectTypes", "GAMEOBJECT_TYPE_"),

                    _ => new()
                };
            }
            else if (typeof(GameobjectTemplateAddon) == modelType)
            {
                return propertyName switch
                {
                    nameof(GameobjectTemplateAddon.Flags) => await GetEnumAsync<TKey>(sharedDefines_path, "GameObjectFlags", "GO_FLAG_"),

                    _ => new()
                };
            }
            else
            {
                return propertyName switch
                {
                    "EquipmentSlots" => await GetEnumAsync<TKey>(player_path, "EquipmentSlots", "EQUIPMENT_SLOT_", "EQUIPMENT_SLOT_START", "EQUIPMENT_SLOT_END"),
                    _ => new()
                };
            }
        }

    }
}
