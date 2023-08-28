using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected async Task<Dictionary<TOptionKey, string>> GetDb2OptionsAsync<TOptionKey>(string db2Name, string valueColumnName, bool convertToFlags = false)
            where TOptionKey : notnull
        {
            return await GetDb2OptionsAsync<TOptionKey, uint>(_appConfig.HotfixesSchema, db2Name, valueColumnName, convertToFlags);
        }

        async Task<Dictionary<TOptionKey, string>> GetDb2OptionsAsync<TOptionKey, TClientKey>(string schemaName, string db2Name, string valueColumnName, bool convertToFlags)
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

                        if (convertToFlags)
                        {
                            // ID of DB2s have so far not been negative nor bigger than int
                            var intKey = int.Parse(key);
                            intKey = intKey == 0 ? 0 : 1 << (intKey - 1);
                            key = intKey.ToString();
                        }

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
                var enumValues = await _serverValuesProvider.GetEnumValues<TOptionKey>(modelType, propertyName);
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

        protected async Task<Dictionary<TOptionKey, string>> GetIconOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            try
            {
                return await _listfileProvider.GetIconsAsync<TOptionKey>();
            }
            catch
            {
                // Log?
                return new();
            }
        }

        protected async Task<Dictionary<TOptionKey, string>> GetDifficultyOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results.InitializeDefaultValue();

            await Task.Run(async () =>
            {
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
            });

            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetTextureFileDataOptionsAsync<TOptionKey>(bool filedataAsId = false)
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results.InitializeDefaultValue();

            await Task.Run(async () =>
            {
                try
                {
                    var textureFileData = await GetAsync(_appConfig.HotfixesSchema, "TextureFileData", false, true);
                    var textureFiles = await _listfileProvider.GetTexturesAsync<int>();

                    foreach (var data in textureFileData)
                    {
                        var key = filedataAsId ? data.GetValueByNameAs<TOptionKey>("FileDataID") : data.GetValueByNameAs<TOptionKey>("MaterialResourcesID");
                        var fileDataId = data.GetValueByNameAs<int>("FileDataID");
                        if (textureFiles.ContainsKey(fileDataId))
                            results[key] = textureFiles[fileDataId];
                    }
                }
                catch
                {
                    // Log?
                }
            });


            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetCreatureModelDataOptionsAsync<TOptionKey>(bool filedataAsId = false)
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results.InitializeDefaultValue();

            await Task.Run(async () =>
            {
                try
                {
                    var creatureModelData = await GetAsync(_appConfig.HotfixesSchema, "CreatureModelData", false, true);
                    var modelFiles = await _listfileProvider.GetModelsAsync<int>();


                    foreach (var data in creatureModelData)
                    {
                        var fileDataId = data.GetValueByNameAs<int>("FileDataID");
                        var key = filedataAsId ? data.GetValueByNameAs<TOptionKey>("FileDataID") : data.GetValueByNameAs<TOptionKey>("ID");

                        if (modelFiles.ContainsKey(fileDataId))
                            results[key] = modelFiles[fileDataId];
                        else
                            results[key] = "Unknown";
                    }
                }
                catch
                {
                    // Log?
                }
            });
            return results;
        }


        protected async Task<Dictionary<TOptionKey, string>> GetModelFileDataOptionsAsync<TOptionKey>(bool filedataAsId = false)
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            results.InitializeDefaultValue();

            await Task.Run(async () =>
            {
                try
                {
                    var modelData = await GetAsync(_appConfig.HotfixesSchema, "ModelFileData", false, true);
                    var modelFiles = await _listfileProvider.GetModelsAsync<int>();


                    foreach (var data in modelData)
                    {
                        var fileDataId = data.GetValueByNameAs<int>("FileDataID");
                        var key = filedataAsId ? data.GetValueByNameAs<TOptionKey>("FileDataID") : data.GetValueByNameAs<TOptionKey>("ModelResourcesID");

                        if (modelFiles.ContainsKey(fileDataId))
                            results[key] = modelFiles[fileDataId];
                        else
                            results[key] = "Unknown";
                    }
                }
                catch
                {
                    // Log?
                }
            });
            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetPlayerConditionOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            return await GetDb2OptionsAsync<TOptionKey>("PlayerCondition", "Failure_Description");
        }

        protected async Task<Dictionary<TOptionKey, string>> GetParticleColorOptionsAsync<TOptionKey>()
            where TOptionKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            var particleColors = await GetAsync(_appConfig.HotfixesSchema, "ParticleColor", false, true);
            foreach (var particleColor in particleColors)
            {
                var colors = $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("Start0"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("Start1"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("Start2"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("MID0"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("MID1"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("MID2"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("End0"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("End1"))}, ";
                colors += $"{Db2Helper.ConvertToHexColor(particleColor.GetValueByNameAs<int>("End2"))}";


                results.Add(particleColor.GetValueByNameAs<TOptionKey>("ID"), colors);
            }
            return results;
        }

        #endregion

    }
}