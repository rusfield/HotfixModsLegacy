﻿/*
 * This client is getting data from the <a href="https://github.com/wowdev/WoWDBDefs">WoWDBDefs repository in GitHub, by wowdev</a>.
 * Code is mostly from <a href="https://github.com/wowdev/DBCD>WoWDev's DBCD repository</a>.
 * Helper methods are based on <a href="https://github.com/MaxtorCoder/Wow.DB2DefinitionDumper>MaxtorCoder's Wow.DB2DefinitionDumper</a>.
 */

using DBDefsLib;
using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using static DBDefsLib.Structs;
using System.Reflection.Metadata;

namespace HotfixMods.Providers.WowDev.Client
{
    public partial class Db2Client : IClientDbProvider, IClientDbDefinitionProvider
    {
        HttpClient _httpClient;

        public string Build { get; set; }

        public Db2Client(string build, string? githubAccessToken = null)
        {
            Build = build;
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "HotfixMods");
            if (!string.IsNullOrWhiteSpace(githubAccessToken))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {githubAccessToken}");
        }

        public async Task<IEnumerable<DbRow>> GetAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(location, db2Name))
                return new List<DbRow>();
            return await ReadDb2FileAsync(location, db2Name, Build, parameters, false);
        }

        public async Task<DbRow?> GetSingleAsync(string location, string db2Name, DbRowDefinition dbRowDefinition, params DbParameter[] parameters)
        {
            if (!await Db2ExistsAsync(location, db2Name))
                return null;
            return (await ReadDb2FileAsync(location, db2Name, Build, parameters, true)).FirstOrDefault();
        }

        public async Task<DbRowDefinition?> GetDefinitionAsync(string location, string db2Name)
        {
            if (string.IsNullOrWhiteSpace(db2Name))
            {
                throw new Exception("Db2 Name and Build must have a value.");
            }
            db2Name = TrimDb2Name(db2Name);
            DBDefinition databaseDefinitions;
            VersionDefinitions versionDefinition;

            try
            {
                // Will crash if definition is missing or does not contain the specified version
                (databaseDefinitions, versionDefinition) = await GetDbDefinitionAndVersionDefinitionsByDb2Name(db2Name, Build);
            }
            catch(Exception e)
            {
                return null;
            }


            var dbRowDefinition = new DbRowDefinition(db2Name);
            foreach (var fieldDefinition in versionDefinition.definitions)
            {
                var columnDefinition = databaseDefinitions.columnDefinitions[fieldDefinition.name];
                var definitionName = fieldDefinition.name.Replace("_lang", "");

                // Remove underscore and set uppercase
                // Assuming name does not start with underscore or contains two underscores after one another
                // Exception is for properties named Field_{patch}
                string name = "";
                if (definitionName.StartsWith("Field"))
                {
                    name = definitionName;
                }
                else
                {
                    bool isUnderscore = false;

                    foreach (var c in definitionName)
                    {
                        if (isUnderscore)
                        {
                            // previous was underscore
                            name += char.ToUpper(c);
                            isUnderscore = false;
                        }
                        else
                        {
                            isUnderscore = c == '_';
                            if (!isUnderscore)
                                name += c;
                        }
                    }
                }


                var type = FieldDefinitionToType(fieldDefinition, columnDefinition);

                if (fieldDefinition.arrLength != 0)
                {
                    for (int i = 0; i<fieldDefinition.arrLength; i++)
                    {
                        var arrayColName = $"{name}{i}";
                        dbRowDefinition.ColumnDefinitions.Add(new()
                        {
                            Name = arrayColName,
                            Type = type,
                            IsIndex = fieldDefinition.isID,
                            IsParentIndex = fieldDefinition.isRelation,
                            ReferenceDb2 = columnDefinition.foreignTable,
                            ReferenceDb2Field = columnDefinition.foreignColumn,
                            IsLocalized = columnDefinition.type == "locstring"
                        });
                    }
                }
                else
                {
                    dbRowDefinition.ColumnDefinitions.Add(new()
                    {
                        Name = name,
                        Type = type,
                        IsIndex = fieldDefinition.isID,
                        IsParentIndex = fieldDefinition.isRelation,
                        ReferenceDb2 = columnDefinition.foreignTable,
                        ReferenceDb2Field = columnDefinition.foreignColumn,
                        IsLocalized = columnDefinition.type == "locstring"
                    });
                }
            }
            dbRowDefinition.ColumnDefinitions.Add(new()
            {
                Name = "VerifiedBuild",
                Type = typeof(int),
                IsIndex = false,
                IsParentIndex = false,
                ReferenceDb2 = null,
                ReferenceDb2Field = null,
                IsLocalized = false
            });
            return dbRowDefinition;
        }

        public async Task<IEnumerable<string>> GetDefinitionNamesAsync()
        {
            return await GetAllDefinitionsFromPathAsync();
            //return await GetAllDefinitionsFromUrlAsync();
        }

        public async Task<IEnumerable<string>> GetAvailableBuildsForDefinitionAsync(string db2Name)
        {
            return await GetBuildsAsync(db2Name);
        }

        public async Task<bool> Db2ExistsAsync(string location, string db2Name)
        {
            if (!location.EndsWith("\\"))
                location += "\\";
            return await Task.Run(() => File.Exists($"{location}{db2Name}.db2"));
        }
    }
}
