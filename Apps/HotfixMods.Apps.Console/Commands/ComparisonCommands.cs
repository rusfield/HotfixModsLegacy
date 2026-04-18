using HotfixMods.Apps.Console.Methods;
using HotfixMods.Tools.Dev.Business;
using System.Reflection;

namespace HotfixMods.Apps.Console.Commands;

internal sealed class CompareServerModelPropertiesCommand : IConsoleCommand
{
    public string Name => "compare-server-models";
    public string Description => "Compare TrinityCore model properties against live MySQL table definitions.";
    public string Usage => "compare-server-models [--assembly <assembly>] [--namespace <namespace>] [--server <host>] [--port <port>] [--user <user>] [--password <password>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var assembly = Assembly.Load(context.Arguments.GetOrDefault("assembly", "HotfixMods.Core"));
        var comparer = new ServerModelPropertyComparer();
        await comparer.CompareAsync(
            context.CreateMySqlClient(),
            assembly,
            context.Arguments.GetOrDefault("namespace", "HotfixMods.Core.Models.TrinityCore"));
    }
}

internal sealed class CompareDb2ModelPropertiesCommand : IConsoleCommand
{
    public string Name => "compare-db2-models";
    public string Description => "Compare DB2 model properties against DBD definitions for a specific build.";
    public string Usage => "compare-db2-models [--assembly <assembly>] [--namespace <namespace>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var assembly = Assembly.Load(context.Arguments.GetOrDefault("assembly", "HotfixMods.Core"));
        var comparer = new ClientModelPropertyComparer();
        await comparer.CompareAsync(
            context.CreateDb2Client(),
            assembly,
            context.Arguments.GetOrDefault("namespace", "HotfixMods.Core.Models.Db2"));
    }
}

internal sealed class CompareServerAndClientDefinitionsCommand : IConsoleCommand
{
    public string Name => "compare-server-and-client-definitions";
    public string Description => "Compare a MySQL table definition with a client DB2 definition.";
    public string Usage => "compare-server-and-client-definitions --schema <schema> --table <table> --db2-name <name> [--server <host>] [--port <port>] [--user <user>] [--password <password>] [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var serverClient = context.CreateMySqlClient();
        var serverDefinition = await serverClient.GetDefinitionAsync(
            context.Arguments.GetRequired("schema"),
            context.Arguments.GetRequired("table"));

        if (serverDefinition is null)
            throw new ConsoleCommandException("No server definition was found for the requested table.");

        var client = context.CreateDb2Client();
        var clientDefinition = await client.GetDefinitionAsync(string.Empty, context.Arguments.GetRequired("db2-name"))
            ?? throw new ConsoleCommandException("No client definition was found for the requested DB2.");

        var tool = new DbDefinitionTool();
        tool.CompareDefinitions(clientDefinition, serverDefinition);
    }
}

internal sealed class InspectTrinityCoreMetadataCommand : IConsoleCommand
{
    public string Name => "inspect-trinitycore-metadata";
    public string Description => "Run the TrinityCore metadata parser helpers against DB2Metadata.h.";
    public string Usage => "inspect-trinitycore-metadata --metadata-path <path> --db2-name <name>";

    public Task ExecuteAsync(ConsoleCommandContext context)
    {
        var tool = new TrinityCoreCodeTool(context.Arguments.GetRequired("metadata-path"));
        var db2Name = context.Arguments.GetRequired("db2-name");
        tool.GetFields(db2Name);
        tool.GetInstanceParameters(db2Name);
        return Task.CompletedTask;
    }
}

internal sealed class ScanHotfixSchemaDependenciesCommand : IConsoleCommand
{
    public string Name => "scan-hotfix-schema-dependencies";
    public string Description => "List external types marked with HotfixesSchema starting from a root type.";
    public string Usage => "scan-hotfix-schema-dependencies --type <type-name> [--assembly <assembly>] [--namespace <namespace>]";

    public Task ExecuteAsync(ConsoleCommandContext context)
    {
        var assemblyName = context.Arguments.GetOrDefault("assembly", "HotfixMods.Infrastructure");
        var namespaceName = context.Arguments.GetOrDefault("namespace", "HotfixMods.Infrastructure.DtoModels");
        var typeName = context.Arguments.GetRequired("type");
        var assembly = Assembly.Load(assemblyName);
        var rootType = assembly.GetType($"{namespaceName}.{typeName}")
            ?? throw new ConsoleCommandException($"Type '{typeName}' was not found in '{namespaceName}' from assembly '{assemblyName}'.");

        var dependencies = HotfixSchemaTypeScanner.GetExternalClassesWithHotfixesSchema(rootType);
        foreach (var dependency in dependencies)
        {
            System.Console.WriteLine(dependency.FullName);
        }

        return Task.CompletedTask;
    }
}
