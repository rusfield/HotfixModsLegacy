using HotfixMods.Core.Enums;
using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.WowDev.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Apps.Console.Methods
{
    public class CustomizationHelper
    {
        Db2Client _db2Client;
        string path = @"D:\TrinityCore\Dragonflight\dbc\enUS";
        string outputPath = @"D:\TrinityCore\Customizations";
        int optionStartId = 10000;
        int choiceStartId = 33000;
        int elementStartId = 120000;
        int hotfixStartId = 901000000;
        int verifiedBuild = -1340;
        uint choiceHash = (uint)TableHashes.CHR_CUSTOMIZATION_CHOICE;
        uint elementHash = (uint)TableHashes.CHR_CUSTOMIZATION_ELEMENT;
        uint optionHash = (uint)TableHashes.CHR_CUSTOMIZATION_OPTION;
        List<ChrModelId> models;
        public CustomizationHelper(Db2Client client)
        {
            _db2Client = client;
            models = new()
            {
                ChrModelId.LIGHTFORGED_DRAENEI_FEMALE,
                ChrModelId.DRAENEI_FEMALE,
                ChrModelId.NIGHTBORNE_FEMALE
            };
        }

        public async Task GenerateAllCustomizationFiles()
        {
            string optionSql = "INSERT INTO hotfixes.chr_customization_option values('{0}', {1}, {1}, 0, {2}, 1000, {3}, {4}, 0, 0, 0, 1000, 90001, " + verifiedBuild + ");";
            string choiceSql = "INSERT INTO hotfixes.chr_customization_choice values('{0}', {1}, {2}, 0, 0, 1000, 1000, 0, 90001, 0, 0, 0, " + verifiedBuild + ", null);";
            string elementSql = "INSERT INTO hotfixes.chr_customization_element values({0}, {1}, 0, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, " + verifiedBuild + ");";
            string hotfixSql = "INSERT INTO hotfixes.hotfix_data values({0}, 0, {1}, {2}, 1, " + verifiedBuild + ");";

            var optionDef = await _db2Client.GetDefinitionAsync(path, "ChrCustomizationOption");
            var choiceDef = await _db2Client.GetDefinitionAsync(path, "ChrCustomizationChoice");
            var elementDef = await _db2Client.GetDefinitionAsync(path, "ChrCustomizationElement");

            var allOptions = await _db2Client.GetAsync(path, "ChrCustomizationOption", optionDef);
            var allChoices = await _db2Client.GetAsync(path, "ChrCustomizationChoice", choiceDef);
            var allElements = await _db2Client.GetAsync(path, "ChrCustomizationElement", elementDef);

            var sortedChoices = new Dictionary<int, List<DbRow>>();
            var sortedElements = new Dictionary<int, List<DbRow>>();

            foreach (var choice in allChoices)
            {
                var optionId = choice.GetValueByNameAs<int>("ChrCustomizationOptionID");
                if (!sortedChoices.ContainsKey(optionId))
                    sortedChoices[optionId] = new List<DbRow>();

                sortedChoices[optionId].Add(choice);
            }

            foreach (var element in allElements)
            {
                var choiceId = element.GetValueByNameAs<int>("ChrCustomizationChoiceID");
                if (!sortedElements.ContainsKey(choiceId))
                    sortedElements[choiceId] = new List<DbRow>();

                sortedElements[choiceId].Add(element);
            }

            foreach (var option in allOptions)
            {
                var optionName = option.GetValueByNameAs<string>("Name");
                if (string.IsNullOrWhiteSpace(optionName))
                    continue;

                foreach (var model in models)
                {
                    var currentPath = Path.Combine(outputPath, $"{model.ToString()} - {((ChrModelId)option.GetValueByNameAs<int>("ChrModelID")).ToString()} - {option.GetValueByNameAs<string>("Name")}.txt");
                    var modelOption = allOptions.Where(o => o.GetValueByNameAs<int>("ChrModelID") == (int)model && o.GetValueByNameAs<string>("Name") == optionName);
                    int categoryId = modelOption.Any() ? modelOption.First().GetValueByNameAs<int>("ChrCustomizationCategoryID") : option.GetValueByNameAs<int>("ChrCustomizationCategoryID");
                    int optionId = -1; // set later

                    using (StreamWriter sw = File.AppendText(currentPath))
                    {
                        if (!sortedChoices.ContainsKey(option.GetIdValue()))
                            continue;

                        // Create option, if not already existing 
                        if (!modelOption.Any())
                        {
                            sw.WriteLine(string.Format(optionSql, optionName, optionStartId, (int)model, categoryId, option.GetValueByNameAs<int>("OptionType")));
                            sw.WriteLine(string.Format(hotfixSql, hotfixStartId++, optionHash, optionStartId));
                            optionId = optionStartId++;
                        }
                        else
                        {
                            optionId = modelOption.First().GetIdValue();
                        }



                        foreach (var choice in sortedChoices[option.GetIdValue()])
                        {
                            var choiceId = choice.GetIdValue();
                            if (!sortedElements.ContainsKey(choiceId))
                                continue;

                            sw.WriteLine(string.Format(choiceSql, choice.GetValueByNameAs<string>("Name"), choiceStartId, optionId));
                            sw.WriteLine(string.Format(hotfixSql, hotfixStartId++, choiceHash, choiceStartId));

                            foreach (var element in sortedElements[choiceId])
                            {
                                sw.WriteLine(string.Format(elementSql,
                                    elementStartId,
                                    choiceStartId,
                                    element.GetValueByNameAs<int>("ChrCustomizationGeosetID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationSkinnedModelID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationMaterialID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationBoneSetID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationCondModelID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationDisplayInfoID"),
                                    element.GetValueByNameAs<int>("ChrCustItemGeoModifyID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationVoiceID"),
                                    element.GetValueByNameAs<int>("AnimKitID")
                                    ));
                                sw.WriteLine(string.Format(hotfixSql, hotfixStartId++, elementHash, elementStartId));

                                elementStartId++;
                            }
                            choiceStartId++;
                        }
                    }
                }

            }
        }
    }
}
