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


        public async Task<Dictionary<TKey, string>> GetEnumValues<TKey>(Type modelType, string propertyName)
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
                    _ => new()
                };

            }

            return new();
        }


    }
}
