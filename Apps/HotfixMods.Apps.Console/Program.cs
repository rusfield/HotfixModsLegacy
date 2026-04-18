using HotfixMods.Apps.Console.Commands;
using HotfixMods.Apps.Console.Configuration;

namespace HotfixMods.Apps.Console;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            var settings = ConsoleAppSettingsLoader.Load(AppContext.BaseDirectory);
            var dispatcher = new ConsoleCommandDispatcher(settings, CreateCommands());
            return await dispatcher.RunAsync(args);
        }
        catch (Exception ex)
        {
            System.Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static IReadOnlyCollection<IConsoleCommand> CreateCommands()
    {
        return
        [
            new CompareDb2ModelPropertiesCommand(),
            new CompareServerAndClientDefinitionsCommand(),
            new CompareServerModelPropertiesCommand(),
            new ConvertDb2HashEnumClipboardCommand(),
            new ConvertWowToolsClipboardCommand(),
            new DumpTransmogSetCommand(),
            new ExportEyeColorsCommand(),
            new GenerateCustomizationsCommand(),
            new GenerateDb2DefinitionClassCommand(),
            new GenerateHotfixDataCommand(),
            new GenerateHotfixTableCommand(),
            new GenerateInfoModelCommand(),
            new GenerateServerDefinitionClassCommand(),
            new ImportDb2DirectoryCommand(),
            new ImportDb2TableCommand(),
            new InspectTrinityCoreMetadataCommand(),
            new ListIconsCommand(),
            new ScanHotfixSchemaDependenciesCommand(),
        ];
    }
}
