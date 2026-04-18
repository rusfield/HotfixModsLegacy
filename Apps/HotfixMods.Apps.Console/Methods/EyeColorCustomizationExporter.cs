using HotfixMods.Core.Enums;
using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.WowDev.Client;
using SystemConsole = System.Console;

namespace HotfixMods.Apps.Console.Methods
{
    public class EyeColorCustomizationExporter
    {
        private readonly Db2Client _db2Client;
        private readonly EyeColorCustomizationExportOptions _options;
        private readonly uint _choiceHash = (uint)TableHashes.CHR_CUSTOMIZATION_CHOICE;
        private readonly uint _elementHash = (uint)TableHashes.CHR_CUSTOMIZATION_ELEMENT;

        public EyeColorCustomizationExporter(Db2Client db2Client, EyeColorCustomizationExportOptions options)
        {
            _db2Client = db2Client;
            _options = options;
        }

        public async Task GenerateAsync()
        {
            var outputDirectory = GetOutputDirectory();
            Directory.CreateDirectory(outputDirectory);

            const string choiceSql = """
                INSERT INTO hotfixes.chr_customization_choice
                (Name, ID, ChrCustomizationOptionID, ChrCustomizationReqID, ChrCustomizationVisReqID, SortOrder, UiOrderIndex, Flags, AddedInPatch, SoundKitID, SwatchColor1, SwatchColor2, VerifiedBuild)
                VALUES ('{0}', {1}, {2}, 146, 0, {3}, {3}, 0, 90001, 0, 0, 0, {4});
                """;
            const string elementSql = """
                INSERT INTO hotfixes.chr_customization_element
                (ID, ChrCustomizationChoiceID, RelatedChrCustomizationChoiceID, ChrCustomizationGeosetID, ChrCustomizationSkinnedModelID, ChrCustomizationMaterialID, ChrCustomizationBoneSetID, ChrCustomizationCondModelID, ChrCustomizationDisplayInfoID, ChrCustItemGeoModifyID, ChrCustomizationVoiceID, AnimKitID, ParticleColorID, ChrCustGeoComponentLinkID, VerifiedBuild)
                VALUES ({0}, {1}, 0, {2}, 0, {3}, 0, 0, 0, 0, 0, 0, 0, 0, {4});
                """;
            string hotfixSql = "INSERT INTO hotfixes.hotfix_data values({0}, 0, {1}, {2}, 1, " + _options.VerifiedBuild + ");";

            var reqDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationReq");
            var optionDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationOption");
            var choiceDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationChoice");
            var elementDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationElement");

            var eyeOptions = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationOption", optionDef, new DbParameter("Name", "Eye Color"));
            var chrCustomizationReq = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationReq", reqDef);
            var eyeElements = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationElement", elementDef);

            var elementData = new Dictionary<int, List<List<(int GeosetId, int MaterialId)>>>();

            foreach (var eyeOption in eyeOptions)
            {
                var model = (ChrModelId)eyeOption.GetValueByNameAs<int>("ChrModelID");

                if (model.ToString().EndsWith("FEMALE", StringComparison.InvariantCulture))
                    continue;

                elementData[(int)model] = new();

                var eyeChoices = await _db2Client.GetAsync(
                    _options.Db2DataPath,
                    "ChrCustomizationChoice",
                    choiceDef,
                    new DbParameter("ChrCustomizationOptionID", eyeOption.GetIdValue()));

                foreach (var eyeChoice in eyeChoices)
                {
                    var orderIndex = eyeChoice.GetValueByNameAs<int>("OrderIndex");
                    if (_options.Excluded.Any(e => model == e.ModelId && orderIndex >= e.From && orderIndex <= e.To))
                        continue;

                    var eyeChoiceReqId = eyeChoice.GetValueByNameAs<int>("ChrCustomizationReqID");
                    var req = chrCustomizationReq.FirstOrDefault(r => r.GetIdValue() == eyeChoiceReqId);
                    if (req is null)
                        continue;

                    if (req.GetValueByNameAs<int>("ClassMask") == 32)
                        continue;

                    if ((req.GetValueByNameAs<int>("ReqType") & 1) == 0)
                        continue;

                    var eyeChoiceId = eyeChoice.GetIdValue();
                    var choiceList = new List<(int GeosetId, int MaterialId)>();

                    foreach (var eyeElement in eyeElements.Where(e => e.GetValueByNameAs<int>("ChrCustomizationChoiceID") == eyeChoiceId))
                    {
                        var materialId = eyeElement.GetValueByNameAs<int>("ChrCustomizationMaterialID");
                        var geosetId = eyeElement.GetValueByNameAs<int>("ChrCustomizationGeosetID");
                        choiceList.Add((geosetId, materialId));
                    }

                    if (choiceList.Any())
                        elementData[(int)model].Add(choiceList);
                }
            }

            foreach (var eyeOption in eyeOptions)
            {
                var model = (ChrModelId)eyeOption.GetValueByNameAs<int>("ChrModelID");
                var modelName = model.ToDisplayString().Replace(" male", "", StringComparison.InvariantCultureIgnoreCase);

                foreach (var elements in elementData)
                {
                    var number = 1;
                    var orderIndex = 1000;
                    var elementModelId = (ChrModelId)elements.Key;
                    var elementModelName = elementModelId.ToDisplayString().Replace(" male", "", StringComparison.InvariantCultureIgnoreCase);
                    var currentPath = Path.Combine(outputDirectory, $"{modelName} - {elementModelName} eyes.txt");

                    using var sw = File.AppendText(currentPath);
                    sw.WriteLine();

                    foreach (var element in elements.Value)
                    {
                        var customizationName = $"{elementModelName} {number++}";
                        var choiceOutput = string.Format(choiceSql, customizationName, _options.ChoiceStartId, eyeOption.GetIdValue(), orderIndex++, _options.VerifiedBuild);
                        var choiceHotfixOutput = string.Format(hotfixSql, _options.HotfixStartId++, _choiceHash, _options.ChoiceStartId);

                        sw.WriteLine(choiceOutput);
                        sw.WriteLine(choiceHotfixOutput);
                        sw.WriteLine();

                        SystemConsole.WriteLine(choiceOutput);
                        SystemConsole.WriteLine(choiceHotfixOutput);
                        SystemConsole.WriteLine();

                        foreach (var data in element)
                        {
                            var elementOutput = string.Format(elementSql, _options.ElementStartId, _options.ChoiceStartId, data.GeosetId, data.MaterialId, _options.VerifiedBuild);
                            var elementHotfixOutput = string.Format(hotfixSql, _options.HotfixStartId++, _elementHash, _options.ElementStartId);

                            sw.WriteLine(elementOutput);
                            sw.WriteLine(elementHotfixOutput);
                            sw.WriteLine();

                            SystemConsole.WriteLine(elementOutput);
                            SystemConsole.WriteLine(elementHotfixOutput);
                            SystemConsole.WriteLine();

                            _options.ElementStartId++;
                        }

                        SystemConsole.WriteLine();
                        _options.ChoiceStartId++;
                    }

                    sw.Flush();
                }
            }
        }

        private string GetOutputDirectory()
        {
            return Path.Combine(_options.OutputPath, $"{nameof(EyeColorCustomizationExporter)}.{nameof(GenerateAsync)}");
        }
    }

    public sealed class EyeColorCustomizationExportOptions
    {
        public string Db2DataPath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public int ChoiceStartId { get; set; } = 33000;
        public int ElementStartId { get; set; } = 120000;
        public int HotfixStartId { get; set; } = 901000000;
        public int VerifiedBuild { get; set; } = -1340;
        public List<(ChrModelId ModelId, int From, int To)> Excluded { get; set; } =
        [
            (ChrModelId.COMPANION_PROTODRAGON, 0, 9999),
            (ChrModelId.COMPANION_PTERRODAX, 0, 9999),
            (ChrModelId.COMPANION_SERPENT, 0, 9999),
            (ChrModelId.COMPANION_WYVERN, 0, 9999),
        ];
    }
}
