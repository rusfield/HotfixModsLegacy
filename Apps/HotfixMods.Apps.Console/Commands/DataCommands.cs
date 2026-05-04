using HotfixMods.Apps.Console.Methods;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.Listfile.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;

namespace HotfixMods.Apps.Console.Commands;

internal sealed class GenerateCustomizationsCommand : IConsoleCommand
{
    public string Name => "generate-customizations";
    public string Description => "Generate customization SQL files from DB2 data.";
    public string Usage => "generate-customizations --db2-path <path> --output-path <path> [--models <ChrModelId,...>] [--option-start-id <int>] [--choice-start-id <int>] [--element-start-id <int>] [--hotfix-start-id <int>] [--verified-build <int>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var options = new CustomizationHelperOptions
        {
            Db2DataPath = context.Arguments.GetRequired("db2-path"),
            OutputPath = context.Arguments.GetRequired("output-path"),
            OptionStartId = context.Arguments.GetInt("option-start-id", 10000),
            ChoiceStartId = context.Arguments.GetInt("choice-start-id", 33000),
            ElementStartId = context.Arguments.GetInt("element-start-id", 120000),
            HotfixStartId = context.Arguments.GetInt("hotfix-start-id", 901000000),
            VerifiedBuild = context.Arguments.GetInt("verified-build", -51340),
            Models = ParseModelIds(context.Arguments.GetCsv("models"))
        };

        var helper = new CustomizationHelper(context.CreateDb2Client(), options);
        await helper.GenerateAllCustomizationFiles();
    }

    private static List<ChrModelId> ParseModelIds(List<string> values)
    {
        if (values.Count == 0)
            return new CustomizationHelperOptions().Models;

        var result = new List<ChrModelId>();
        foreach (var value in values)
        {
            if (!Enum.TryParse<ChrModelId>(value, true, out var model))
                throw new ConsoleCommandException($"Unknown ChrModelId '{value}'.");

            result.Add(model);
        }

        return result;
    }
}

internal sealed class ExportEyeColorsCommand : IConsoleCommand
{
    public string Name => "export-eye-colors";
    public string Description => "Generate eye-color customization SQL output.";
    public string Usage => "export-eye-colors --db2-path <path> --output-path <path> [--choice-start-id <int>] [--element-start-id <int>] [--hotfix-start-id <int>] [--verified-build <int>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var configuredOptions = context.EyeColorExport;
        var options = new EyeColorCustomizationExportOptions
        {
            Db2DataPath = context.Arguments.GetOrDefault("db2-path", configuredOptions.Db2Path),
            OutputPath = context.Arguments.GetOrDefault("output-path", configuredOptions.OutputPath),
            ChoiceStartId = context.Arguments.GetInt("choice-start-id", configuredOptions.ChoiceStartId),
            ElementStartId = context.Arguments.GetInt("element-start-id", configuredOptions.ElementStartId),
            HotfixStartId = context.Arguments.GetInt("hotfix-start-id", configuredOptions.HotfixStartId),
            VerifiedBuild = context.Arguments.GetInt("verified-build", configuredOptions.VerifiedBuild),
        };

        if (string.IsNullOrWhiteSpace(options.Db2DataPath))
            throw new ConsoleCommandException("Missing DB2 path. Pass '--db2-path <path>' or set 'EyeColorExport:Db2Path' in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.OutputPath))
            throw new ConsoleCommandException("Missing output path. Pass '--output-path <path>' or set 'EyeColorExport:OutputPath' in appsettings.json.");

        var exporter = new EyeColorCustomizationExporter(context.CreateDb2Client(), options);
        await exporter.GenerateAsync();
    }
}

internal sealed class ExportCustomizationRequirementOverridesCommand : IConsoleCommand
{
    public string Name => "export-customization-requirement-overrides";
    public string Description => "Generate customization choice overrides that clear nonzero ChrCustomizationReqID values.";
    public string Usage => "export-customization-requirement-overrides --db2-path <path> --output-path <path> [--hotfix-start-id <int>] [--verified-build <int>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var configuredOptions = context.CustomizationRequirementOverride;
        var options = new CustomizationRequirementOverrideOptions
        {
            Db2DataPath = context.Arguments.GetOrDefault("db2-path", configuredOptions.Db2Path),
            OutputPath = context.Arguments.GetOrDefault("output-path", configuredOptions.OutputPath),
            HotfixStartId = context.Arguments.GetInt("hotfix-start-id", configuredOptions.HotfixStartId),
            VerifiedBuild = context.Arguments.GetInt("verified-build", configuredOptions.VerifiedBuild),
        };

        if (string.IsNullOrWhiteSpace(options.Db2DataPath))
            throw new ConsoleCommandException("Missing DB2 path. Pass '--db2-path <path>' or set 'CustomizationRequirementOverride:Db2Path' in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.OutputPath))
            throw new ConsoleCommandException("Missing output path. Pass '--output-path <path>' or set 'CustomizationRequirementOverride:OutputPath' in appsettings.json.");

        var exporter = new CustomizationRequirementOverrideExporter(context.CreateDb2Client(), options);
        await exporter.GenerateAsync();
    }
}

internal sealed class CustomizationRequirementUnlocksCommand : IConsoleCommand
{
    public string Name => "customization-requirement-unlocks";
    public string Description => "Generate customization requirement overrides that clear unlock and NPC-only gates.";
    public string Usage => "customization-requirement-unlocks --db2-path <path> --output-path <path> [--hotfix-start-id <int>] [--verified-build <int>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var configuredOptions = context.CustomizationRequirementUnlock;
        var options = new CustomizationRequirementUnlockOptions
        {
            Db2DataPath = context.Arguments.GetOrDefault("db2-path", configuredOptions.Db2Path),
            OutputPath = context.Arguments.GetOrDefault("output-path", configuredOptions.OutputPath),
            HotfixStartId = context.Arguments.GetInt("hotfix-start-id", configuredOptions.HotfixStartId),
            VerifiedBuild = context.Arguments.GetInt("verified-build", configuredOptions.VerifiedBuild),
        };

        if (string.IsNullOrWhiteSpace(options.Db2DataPath))
            throw new ConsoleCommandException("Missing DB2 path. Pass '--db2-path <path>' or set 'CustomizationRequirementUnlock:Db2Path' in appsettings.json.");

        if (string.IsNullOrWhiteSpace(options.OutputPath))
            throw new ConsoleCommandException("Missing output path. Pass '--output-path <path>' or set 'CustomizationRequirementUnlock:OutputPath' in appsettings.json.");

        var tool = new CustomizationRequirementUnlockTool(context.CreateDb2Client(), options);
        await tool.GenerateAsync();
    }
}

internal sealed class ImportDb2DirectoryCommand : IConsoleCommand
{
    public string Name => "import-db2-directory";
    public string Description => "Import every DB2 file in a directory into MySQL.";
    public string Usage => "import-db2-directory --db2-path <path> --schema <schema> [--build <client-build>] [--server <host>] [--port <port>] [--user <user>] [--password <password>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var db2Path = context.Arguments.GetRequired("db2-path");
        var schema = context.Arguments.GetRequired("schema");
        var importTool = new Db2ImportTool();

        foreach (var file in Directory.EnumerateFiles(db2Path, "*.db2", SearchOption.TopDirectoryOnly))
        {
            var name = Path.GetFileNameWithoutExtension(file);
            try
            {
                await importTool.Db2FileToDb2MySql(
                    context.RequireBuild(),
                    context.RequireDefinitionsPath(),
                    db2Path,
                    name,
                    schema,
                    name,
                    context.Arguments.GetOrDefault("server", context.Settings.MySql.Server),
                    context.Arguments.GetOrDefault("port", context.Settings.MySql.Port),
                    context.Arguments.GetOrDefault("user", context.Settings.MySql.User),
                    context.Arguments.GetOrDefault("password", context.Settings.MySql.Password));
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine($"Failed to import {name}: {ex.Message}");
            }
        }
    }
}

internal sealed class ImportDb2TableCommand : IConsoleCommand
{
    public string Name => "import-db2-table";
    public string Description => "Import a single DB2 file into MySQL.";
    public string Usage => "import-db2-table --db2-path <path> --db2-name <name> --schema <schema> [--table <table>] [--build <client-build>] [--server <host>] [--port <port>] [--user <user>] [--password <password>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var tool = new Db2ImportTool();
        var db2Name = context.Arguments.GetRequired("db2-name");
        await tool.Db2FileToDb2MySql(
            context.RequireBuild(),
            context.RequireDefinitionsPath(),
            context.Arguments.GetRequired("db2-path"),
            db2Name,
            context.Arguments.GetRequired("schema"),
            context.Arguments.GetOrDefault("table", db2Name.ToTableName()),
            context.Arguments.GetOrDefault("server", context.Settings.MySql.Server),
            context.Arguments.GetOrDefault("port", context.Settings.MySql.Port),
            context.Arguments.GetOrDefault("user", context.Settings.MySql.User),
            context.Arguments.GetOrDefault("password", context.Settings.MySql.Password));
    }
}

internal sealed class ListIconsCommand : IConsoleCommand
{
    public string Name => "list-icons";
    public string Description => "Fetch and print icon file data IDs from the listfile provider.";
    public string Usage => "list-icons [--listfile-path <path>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var listfileProvider = new ListfileClient(context.RequireListfilePath());
        var icons = await listfileProvider.GetIconsAsync<int>();

        foreach (var icon in icons)
            System.Console.WriteLine(icon.Value);
    }
}

internal sealed class DumpTransmogSetCommand : IConsoleCommand
{
    public string Name => "dump-transmog-set";
    public string Description => "Print SQL for a TransmogSet-style DB2 export.";
    public string Usage => "dump-transmog-set [--db2-path <path>] [--db2-name <name>] [--verified-build <int>] [--hotfix-start-id <int>] [--build <client-build>] [--server <host>] [--port <port>] [--user <user>] [--password <password>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var db2Name = context.Arguments.GetOrDefault("db2-name", "TransmogSet");
        var db2Path = context.RequireDb2Path();
        var verifiedBuild = context.Arguments.GetInt("verified-build", -51337);
        var hotfixId = await ResolveHotfixStartIdAsync(context);
        var client = context.CreateDb2Client();
        var definition = await client.GetDefinitionAsync(db2Path, db2Name)
            ?? throw new ConsoleCommandException($"No definition found for '{db2Name}'.");
        var data = await client.GetAsync(db2Path, db2Name, definition);
        var tableHash = (uint)TableHashes.TRANSMOG_SET;

        System.Console.WriteLine($"SET @VerifiedBuild = {verifiedBuild};");

        foreach (var row in data)
        {
            var name = row.GetValueByNameAs<string>("Name").Replace("'", "''");
            var recordId = row.GetIdValue();
            System.Console.WriteLine(
                $"replace into hotfixes.transmog_set (Name, ID, ClassMask, TrackingQuestID, Flags, TransmogSetGroupID, ItemNameDescriptionID, ParentTransmogSetID, Unknown810, ExpansionID, PatchID, UiOrder, PlayerConditionID, VerifiedBuild) values('{name}', {recordId}, 0, 0, 0, 0, 0, 0, 1, {row.GetValueByNameAs<string>("ExpansionID")}, 90205, {row.GetValueByNameAs<string>("UiOrder")}, 0, @VerifiedBuild);");
            System.Console.WriteLine(
                $"update hotfixes.hotfix_data set Status = 4 where TableHash = {tableHash} and RecordId = {recordId} and Status = 1;");
            System.Console.WriteLine(
                $"insert into hotfixes.hotfix_data (Id, UniqueId, TableHash, RecordId, Status, VerifiedBuild) values({hotfixId}, 0, {tableHash}, {recordId}, 1, @VerifiedBuild);");
            hotfixId++;
        }
    }

    private static async Task<int> ResolveHotfixStartIdAsync(ConsoleCommandContext context)
    {
        var configuredStartId = context.Arguments.Get("hotfix-start-id");
        if (int.TryParse(configuredStartId, out var hotfixStartId))
            return hotfixStartId;

        try
        {
            var mySqlClient = context.CreateMySqlClient();
            var highestHotfixId = await mySqlClient.GetHighestIdAsync("hotfixes", "hotfix_data", 0, int.MaxValue, "Id");
            if (highestHotfixId == int.MaxValue)
                throw new ConsoleCommandException("hotfixes.hotfix_data is full. Pass '--hotfix-start-id <int>' to override.");

            return highestHotfixId + 1;
        }
        catch (ConsoleCommandException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConsoleCommandException(
                $"Unable to determine the next hotfix_data ID from MySQL ({ex.Message}). Pass '--hotfix-start-id <int>' or configure MySQL connection settings in appsettings.json.");
        }
    }
}

internal sealed class GenerateHotfixDataCommand : IConsoleCommand
{
    public string Name => "generate-hotfix-data";
    public string Description => "Generate hotfix_data insert statements from a hash and ID list.";
    public string Usage => "generate-hotfix-data --hash <decimal-or-hex> --ids <id1,id2,...> [--start-id <int>]";

    public Task ExecuteAsync(ConsoleCommandContext context)
    {
        var hashInput = context.Arguments.GetRequired("hash");
        var ids = context.Arguments.GetCsv("ids");
        if (ids.Count == 0)
            throw new ConsoleCommandException("Pass at least one ID with '--ids <id1,id2,...>'.");

        var startId = context.Arguments.GetInt("start-id", 604000);
        var hash = ParseHash(hashInput);
        foreach (var id in ids.Select(int.Parse))
        {
            System.Console.WriteLine($"insert into hotfix_data values({startId + id}, 0, {hash}, {id}, 1, -51337);");
        }

        return Task.CompletedTask;
    }

    private static string ParseHash(string input)
    {
        if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return input;

        return uint.Parse(input).ToString();
    }
}
