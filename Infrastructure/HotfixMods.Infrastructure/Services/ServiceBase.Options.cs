﻿using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected async Task<Dictionary<TOptionKey, string>> GetDb2OptionsAsync<TOptionKey>(string db2Name, string valueColumnName)
            where TOptionKey : notnull
        {
            return await GetDb2OptionsAsync<TOptionKey, uint>(_appConfig.HotfixesSchema, db2Name, valueColumnName);
        }

        async Task<Dictionary<TOptionKey, string>> GetDb2OptionsAsync<TOptionKey, TClientKey>(string schemaName, string db2Name, string valueColumnName)
            where TOptionKey : notnull
            where TClientKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            await Task.Run(async () =>
            {
                results.InitializeDefaultValue();
                var options = await GetAsync(schemaName, db2Name, false, true);
                foreach (var option in options)
                {
                    var key = option.Columns.Where(c => c.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Value?.ToString();
                    var value = option.Columns.Where(c => c.Name.Equals(valueColumnName, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault()?.Value?.ToString();

                    if (key != null)
                    {
                        if (string.IsNullOrWhiteSpace(value))
                            value = key;
                        else
                            value = $"{value}";


                        var optionKey = (TOptionKey)Convert.ChangeType(key, typeof(TOptionKey));
                        results[optionKey] = value;
                    }
                }
            });
            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetEnumOptionsAsync<TOptionKey>(Type? modelType, string propertyName)
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            await Task.Run(async () =>
            {
                var enumValues = await _serverEnumProvider.GetEnumValues<TOptionKey>(modelType, propertyName);
                results.InitializeDefaultValue();

                for (int i = 0; i < enumValues.Count; i++)
                {
                    var item = enumValues.ElementAt(i);
                    results[item.Key] = $"{item.Value}";
                }
            });
            return results;
        }


        #region Shared Special Options
        protected async Task<Dictionary<TOptionKey, string>> GetFactionOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results.InitializeDefaultValue();
            await Task.Run(async () =>
            {
                var factions = (await GetAsync(_appConfig.HotfixesSchema, "Faction", false, true)).ToDictionary(k => k.GetIdValue(), v => v.GetValueByNameAs<string>("Name"));
                var factionTemplates = await GetAsync(_appConfig.HotfixesSchema, "FactionTemplate", false, true);

                foreach (var factionTemplate in factionTemplates)
                {
                    var id = factionTemplate.GetIdValue();
                    string displayName = "";
                    if (factions.ContainsKey(id))
                        displayName = $"{factions[id]}";

                    var key = (TOptionKey)Convert.ChangeType(id, typeof(TOptionKey));
                    results.Add(key, displayName);
                }
            });

            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetIconFileDataIdOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            return await _listfileProvider.GetIconsAsync<TOptionKey>();
        }

        protected async Task<Dictionary<TOptionKey, string>> GetDifficultyIdOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results[default(TOptionKey)] = "None";
            var mapTypes = await GetEnumOptionsAsync<byte>(typeof(SpellAuraOptions), nameof(SpellAuraOptions.DifficultyID));
            var difficulties = await GetAsync(_appConfig.HotfixesSchema, "Difficulty", false, true);

            foreach (var difficulty in difficulties)
            {
                var instanceType = difficulty.GetValueByNameAs<byte>("InstanceType");
                var name = difficulty.GetValueByNameAs<string>("Name");
                if (mapTypes.ContainsKey(instanceType))
                {
                    var mapType = mapTypes[instanceType] ?? "";
                    name = name.Replace(mapType, "", StringComparison.InvariantCultureIgnoreCase);
                    name = $"{name} {mapType}";
                }

                results.Add((TOptionKey)Convert.ChangeType(difficulty.GetIdValue().ToString(), typeof(TOptionKey)), name);
            }

            return results;
        }
        #endregion
    }
}