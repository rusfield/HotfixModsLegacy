using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models.TrinityCore;
using System.Security.Cryptography.X509Certificates;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient : IServerEnumProvider
    {
        public TrinityCoreClient(string trinityCorePath) 
        {
            TrinityCorePath = trinityCorePath;
        }

        public string TrinityCorePath { get; set; } = "/";


        public async Task<Dictionary<TKey, string>> GetEnumValues<TKey>(Type? modelType, string propertyName)
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
                    nameof(CreatureTemplate.HealthScalingExpansion) or nameof(CreatureTemplate.RequiredExpansion) => await GetEnumAsync<TKey>(sharedDefines_path, "Expansions", "EXPANSION_", "EXPANSION_LEVEL_CURRENT"),


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
