﻿using HotfixMods.Core.Interfaces;
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
                if (Equal(nameof(CreatureTemplate.Rank), propertyName))
                {
                    return await GetEnumAsync<TKey>(sharedDefines_path, "CreatureEliteType", "CREATURE_ELITE_", "CREATURE_");
                }
                else if(Equal(nameof(CreatureTemplate.MovementType), propertyName))
                {
                    return await GetEnumAsync<TKey>(movement_path, "MovementGeneratorType", "_MOTION_TYPE");
                }
            }

            return new();
        }


    }
}
