using HotfixMods.Core.Enums;
using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.WowDev.Client;
using SystemConsole = System.Console;

namespace HotfixMods.Apps.Console.Methods
{
    public sealed class CustomizationRequirementOverrideExporter
    {
        private readonly Db2Client _db2Client;
        private readonly CustomizationRequirementOverrideOptions _options;
        private readonly uint _choiceHash = (uint)TableHashes.CHR_CUSTOMIZATION_CHOICE;

        public CustomizationRequirementOverrideExporter(
            Db2Client db2Client,
            CustomizationRequirementOverrideOptions options)
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
                VALUES ('{0}', {1}, {2}, 0, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11});
                """;
            string hotfixSql = "INSERT INTO hotfixes.hotfix_data values({0}, 0, {1}, {2}, 1, " + _options.VerifiedBuild + ");";

            var optionDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationOption");
            var choiceDef = await _db2Client.GetDefinitionAsync(_options.Db2DataPath, "ChrCustomizationChoice");
            var customizationOptions = await _db2Client.GetAsync(_options.Db2DataPath, "ChrCustomizationOption", optionDef);

            foreach (var customizationOption in customizationOptions)
            {
                var optionName = customizationOption.GetValueByNameAs<string>("Name");
                if (string.IsNullOrWhiteSpace(optionName))
                    continue;

                var model = (ChrModelId)customizationOption.GetValueByNameAs<int>("ChrModelID");
                var currentPath = Path.Combine(
                    outputDirectory,
                    $"{model.ToDisplayString()} - {SanitizeFileName(optionName)} requirement overrides.txt");

                var choices = await _db2Client.GetAsync(
                    _options.Db2DataPath,
                    "ChrCustomizationChoice",
                    choiceDef,
                    new DbParameter("ChrCustomizationOptionID", customizationOption.GetIdValue()));

                using var sw = File.AppendText(currentPath);

                foreach (var choice in choices)
                {
                    if (choice.GetValueByNameAs<int>("ChrCustomizationReqID") == 0)
                        continue;

                    var choiceOutput = string.Format(
                        choiceSql,
                        EscapeSql(choice.GetValueByNameAs<string>("Name")),
                        choice.GetIdValue(),
                        choice.GetValueByNameAs<int>("ChrCustomizationOptionID"),
                        choice.GetValueByNameAs<int>("ChrCustomizationVisReqID"),
                        choice.GetValueByNameAs<int>("OrderIndex"),
                        choice.GetValueByNameAs<int>("UiOrderIndex"),
                        choice.GetValueByNameAs<int>("Flags"),
                        choice.GetValueByNameAs<int>("AddedInPatch"),
                        choice.GetValueByNameAs<int>("SoundKitID"),
                        choice.GetValueByNameAs<int>("SwatchColor0"),
                        choice.GetValueByNameAs<int>("SwatchColor1"),
                        _options.VerifiedBuild);
                    var hotfixOutput = string.Format(hotfixSql, _options.HotfixStartId++, _choiceHash, choice.GetIdValue());

                    sw.WriteLine(choiceOutput);
                    sw.WriteLine(hotfixOutput);
                    sw.WriteLine();

                    SystemConsole.WriteLine(choiceOutput);
                    SystemConsole.WriteLine(hotfixOutput);
                    SystemConsole.WriteLine();
                }

                sw.Flush();
            }
        }

        private static string EscapeSql(string value)
        {
            return value.Replace("'", "''");
        }

        private static string SanitizeFileName(string value)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                value = value.Replace(invalidChar, '_');

            return value;
        }

        private string GetOutputDirectory()
        {
            return Path.Combine(_options.OutputPath, $"{nameof(CustomizationRequirementOverrideExporter)}.{nameof(GenerateAsync)}");
        }
    }

    public sealed class CustomizationRequirementOverrideOptions
    {
        public string Db2DataPath { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
        public int HotfixStartId { get; set; } = 902000000;
        public int VerifiedBuild { get; set; } = -51340;
    }
}
