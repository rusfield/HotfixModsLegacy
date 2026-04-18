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
        private readonly Db2Client _db2Client;
        private readonly CustomizationHelperOptions _options;
        private readonly uint _choiceHash = (uint)TableHashes.CHR_CUSTOMIZATION_CHOICE;
        private readonly uint _elementHash = (uint)TableHashes.CHR_CUSTOMIZATION_ELEMENT;
        private readonly uint _optionHash = (uint)TableHashes.CHR_CUSTOMIZATION_OPTION;

        public CustomizationHelper(Db2Client client, CustomizationHelperOptions options)
        {
            _db2Client = client;
            _options = options;
        }

        public async Task GenerateAllCustomizationFiles()
        {
            Directory.CreateDirectory(_options.OutputPath);

            string optionSql = "INSERT INTO hotfixes.chr_customization_option values('{0}', {1}, {1}, 0, {2}, 1000, {3}, {4}, 0, 0, 0, 1000, 90001, " + _options.VerifiedBuild + ");";
            const string choiceSql = """
                INSERT INTO hotfixes.chr_customization_choice
                (Name, ID, ChrCustomizationOptionID, ChrCustomizationReqID, ChrCustomizationVisReqID, SortOrder, UiOrderIndex, Flags, AddedInPatch, SoundKitID, SwatchColor1, SwatchColor2, VerifiedBuild)
                VALUES ('{0}', {1}, {2}, 0, 0, 1000, 1000, 0, 90001, 0, 0, 0, {3});
                """;
            const string elementSql = """
                INSERT INTO hotfixes.chr_customization_element
                (ID, ChrCustomizationChoiceID, RelatedChrCustomizationChoiceID, ChrCustomizationGeosetID, ChrCustomizationSkinnedModelID, ChrCustomizationMaterialID, ChrCustomizationBoneSetID, ChrCustomizationCondModelID, ChrCustomizationDisplayInfoID, ChrCustItemGeoModifyID, ChrCustomizationVoiceID, AnimKitID, ParticleColorID, ChrCustGeoComponentLinkID, VerifiedBuild)
                VALUES ({0}, {1}, 0, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, 0, 0, {11});
                """;
            string hotfixSql = "INSERT INTO hotfixes.hotfix_data values({0}, 0, {1}, {2}, 1, " + _options.VerifiedBuild + ");";

            var optionDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationOption");
            var choiceDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationChoice");
            var elementDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationElement");

            var allOptions = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationOption", optionDef);
            var allChoices = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationChoice", choiceDef);
            var allElements = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationElement", elementDef);

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

                foreach (var model in _options.Models)
                {
                    var currentPath = Path.Combine(_options.OutputPath, $"{model.ToString()} - {((ChrModelId)option.GetValueByNameAs<int>("ChrModelID")).ToString()} - {option.GetValueByNameAs<string>("Name")}.txt");
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
                            sw.WriteLine(string.Format(optionSql, optionName, _options.OptionStartId, (int)model, categoryId, option.GetValueByNameAs<int>("OptionType")));
                            sw.WriteLine(string.Format(hotfixSql, _options.HotfixStartId++, _optionHash, _options.OptionStartId));
                            optionId = _options.OptionStartId++;
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

                            sw.WriteLine(string.Format(choiceSql, choice.GetValueByNameAs<string>("Name"), _options.ChoiceStartId, optionId, _options.VerifiedBuild));
                            sw.WriteLine(string.Format(hotfixSql, _options.HotfixStartId++, _choiceHash, _options.ChoiceStartId));

                            foreach (var element in sortedElements[choiceId])
                            {
                                sw.WriteLine(string.Format(elementSql,
                                    _options.ElementStartId,
                                    _options.ChoiceStartId,
                                    element.GetValueByNameAs<int>("ChrCustomizationGeosetID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationSkinnedModelID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationMaterialID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationBoneSetID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationCondModelID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationDisplayInfoID"),
                                    element.GetValueByNameAs<int>("ChrCustItemGeoModifyID"),
                                    element.GetValueByNameAs<int>("ChrCustomizationVoiceID"),
                                    element.GetValueByNameAs<int>("AnimKitID"),
                                    _options.VerifiedBuild
                                    ));
                                sw.WriteLine(string.Format(hotfixSql, _options.HotfixStartId++, _elementHash, _options.ElementStartId));

                                _options.ElementStartId++;
                            }
                            _options.ChoiceStartId++;
                        }
                    }
                }

            }
        }
    }

    public sealed class CustomizationHelperOptions
    {
        public string Db2DataPath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public int OptionStartId { get; set; } = 10000;
        public int ChoiceStartId { get; set; } = 33000;
        public int ElementStartId { get; set; } = 120000;
        public int HotfixStartId { get; set; } = 901000000;
        public int VerifiedBuild { get; set; } = -1340;
        public List<ChrModelId> Models { get; set; } =
        [
            ChrModelId.LIGHTFORGED_DRAENEI_FEMALE,
            ChrModelId.DRAENEI_FEMALE,
            ChrModelId.NIGHTBORNE_FEMALE
        ];
    }
}
