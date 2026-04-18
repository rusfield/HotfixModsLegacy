using HotfixMods.Apps.Console.Methods;
using HotfixMods.Tools.Dev.Business;
using System.Reflection;

namespace HotfixMods.Apps.Console.Commands;

internal sealed class GenerateDb2DefinitionClassCommand : IConsoleCommand
{
    public string Name => "generate-db2-definition-class";
    public string Description => "Generate a C# class from a DBD definition.";
    public string Usage => "generate-db2-definition-class --name <db2-name> [--build <client-build>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var tool = new DbDefinitionTool();
        await tool.DefinitionToCSharp(context.Arguments.GetRequired("name"), context.RequireBuild(), context.RequireDefinitionsPath());
    }
}

internal sealed class GenerateServerDefinitionClassCommand : IConsoleCommand
{
    public string Name => "generate-server-definition-class";
    public string Description => "Generate a C# class from a MySQL table definition.";
    public string Usage => "generate-server-definition-class --schema <schema> --table <table> [--server <host>] [--port <port>] [--user <user>] [--password <password>]";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var definition = await context.CreateMySqlClient().GetDefinitionAsync(
            context.Arguments.GetRequired("schema"),
            context.Arguments.GetRequired("table"));

        if (definition is null)
            throw new ConsoleCommandException("No server definition was found for the requested table.");

        var tool = new TrinityCoreDbTool();
        await tool.DbDefToCSharp(definition);
    }
}

internal sealed class GenerateHotfixTableCommand : IConsoleCommand
{
    public string Name => "generate-hotfix-table";
    public string Description => "Generate TrinityCore DB2 storage snippets for one or more DB2 model types.";
    public string Usage => "generate-hotfix-table --types <Type1,Type2,...> [--assembly <assembly>] [--namespace <namespace>]";

    public Task ExecuteAsync(ConsoleCommandContext context)
    {
        var types = ConsoleTypeResolver.ResolveTypes(
            assemblyName: context.Arguments.GetOrDefault("assembly", "HotfixMods.Core"),
            namespaceName: context.Arguments.GetOrDefault("namespace", "HotfixMods.Core.Models.Db2"),
            typeNames: context.Arguments.GetCsv("types"));

        if (types.Length == 0)
            throw new ConsoleCommandException("Pass at least one type with '--types <Type1,Type2,...>'.");

        var tool = new HotfixTableTool();
        tool.GenerateAll(types);
        return Task.CompletedTask;
    }
}

internal sealed class ConvertDb2HashEnumClipboardCommand : IConsoleCommand
{
    public string Name => "convert-db2-hash-clipboard";
    public string Description => "Convert TrinityCore DB2 hash clipboard content into C# enum values.";
    public string Usage => "convert-db2-hash-clipboard";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var tool = new TrinityCoreDbTool();
        await tool.Db2HashEnumInClipboardToCSharp();
    }
}

internal sealed class ConvertWowToolsClipboardCommand : IConsoleCommand
{
    public string Name => "convert-wowtools-clipboard";
    public string Description => "Convert WowTools clipboard content into C# enums, flags, or arrays.";
    public string Usage => "convert-wowtools-clipboard --mode <enum|flag|array>";

    public async Task ExecuteAsync(ConsoleCommandContext context)
    {
        var mode = context.Arguments.GetRequired("mode");
        var wowToolsTool = new WowToolsTool();

        switch (mode.ToLowerInvariant())
        {
            case "enum":
                await wowToolsTool.EnumInClipboardToCSharp();
                break;
            case "flag":
                await wowToolsTool.FlagInClipboardToCSharp();
                break;
            case "array":
                await wowToolsTool.ArrayInClipboardToCSharp();
                break;
            default:
                throw new ConsoleCommandException("Invalid mode. Use 'enum', 'flag', or 'array'.");
        }
    }
}

internal sealed class GenerateInfoModelCommand : IConsoleCommand
{
    public string Name => "generate-info-model";
    public string Description => "Generate an info-model scaffold from a DTO type.";
    public string Usage => "generate-info-model --dto <type-name> [--assembly <assembly>] [--namespace <namespace>]";

    public Task ExecuteAsync(ConsoleCommandContext context)
    {
        var assemblyName = context.Arguments.GetOrDefault("assembly", "HotfixMods.Infrastructure");
        var namespaceName = context.Arguments.GetOrDefault("namespace", "HotfixMods.Infrastructure.DtoModels");
        var typeName = context.Arguments.GetRequired("dto");
        var assembly = Assembly.Load(assemblyName);
        var dtoType = assembly.GetType($"{namespaceName}.{typeName}")
            ?? throw new ConsoleCommandException($"DTO '{typeName}' was not found in assembly '{assemblyName}'.");

        new InfoModelGenerator().Generate(dtoType);
        return Task.CompletedTask;
    }
}

internal static class ConsoleTypeResolver
{
    public static Type[] ResolveTypes(string assemblyName, string namespaceName, List<string> typeNames)
    {
        var assembly = Assembly.Load(assemblyName);
        return typeNames
            .Select(typeName => assembly.GetType($"{namespaceName}.{typeName}")
                ?? throw new ConsoleCommandException($"Type '{typeName}' was not found in '{namespaceName}' from assembly '{assemblyName}'."))
            .ToArray();
    }
}
