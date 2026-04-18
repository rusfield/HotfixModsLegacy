using HotfixMods.Core.Attributes;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using System.Reflection;

namespace HotfixMods.Apps.Console.Methods;

public sealed class ServerModelPropertyComparer
{
    public async Task CompareAsync(MySqlClient serverDefHelper, Assembly assembly, string modelNamespace)
    {
        var serverModels = assembly.GetTypes().Where(t => t.Namespace == modelNamespace).ToList();
        foreach (var model in serverModels)
        {
            System.Console.WriteLine($"Checking {model.Name}");
            var properties = model.GetProperties().Select(p => p.Name).ToList();

            var schema = GetSchemaName(model);
            if (string.IsNullOrWhiteSpace(schema))
            {
                System.Console.WriteLine("No schema found for " + model.Name);
                continue;
            }

            var definition = await serverDefHelper.GetDefinitionAsync(schema, model.Name.ToTableName());
            if (definition is null)
            {
                System.Console.WriteLine("No definitions found for " + model.Name);
                continue;
            }

            var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
            var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

            foreach (var prop in model.GetProperties())
            {
                var defProp = definition.ColumnDefinitions.FirstOrDefault(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase));
                if (defProp is null)
                {
                    System.Console.WriteLine($"{prop.Name} not found");
                    continue;
                }

                if (prop.PropertyType != defProp.Type)
                {
                    System.Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
                }
            }

            if (oldNames.Any() || newDefinitions.Any())
            {
                System.Console.WriteLine($"Updated values for {model.Name}");
                System.Console.WriteLine("New names:");
                foreach (var definitionName in newDefinitions)
                {
                    System.Console.WriteLine(definitionName.Name);
                }

                System.Console.WriteLine("Old names:");
                foreach (var oldName in oldNames)
                {
                    System.Console.WriteLine(oldName);
                }

                System.Console.WriteLine();
            }
            else
            {
                System.Console.WriteLine($"{model.Name} OK");
            }
        }
    }

    private static string GetSchemaName(MemberInfo model)
    {
        if (model.GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
            return "hotfixes";

        if (model.GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
            return "world";

        if (model.GetCustomAttribute(typeof(HotfixModsSchemaAttribute)) != null)
            return "hotfixMods";

        if (model.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
            return "characters";

        return string.Empty;
    }
}

public sealed class ClientModelPropertyComparer
{
    public async Task CompareAsync(Db2Client db2Client, Assembly assembly, string modelNamespace)
    {
        var models = assembly.GetTypes().Where(t => t.Namespace == modelNamespace).ToList();

        foreach (var model in models)
        {
            var properties = model.GetProperties().Select(p => p.Name).ToList();
            try
            {
                System.Console.WriteLine($"Checking {model.Name}");
                var definition = await db2Client.GetDefinitionAsync(string.Empty, model.Name);
                if (definition is null)
                {
                    System.Console.WriteLine("No definitions found for " + model.Name);
                    continue;
                }

                var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
                var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

                foreach (var prop in model.GetProperties())
                {
                    var defProp = definition.ColumnDefinitions.FirstOrDefault(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (defProp is null)
                    {
                        System.Console.WriteLine($"{prop.Name} not found");
                        continue;
                    }

                    if (prop.PropertyType != defProp.Type)
                    {
                        System.Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
                    }
                }

                if (oldNames.Any() || newDefinitions.Any())
                {
                    System.Console.WriteLine($"Updated values for {model.Name}");
                    System.Console.WriteLine("New names:");
                    foreach (var definitionName in newDefinitions)
                    {
                        System.Console.WriteLine($"{definitionName.Name} ({definitionName.Type})");
                    }

                    System.Console.WriteLine("Old names:");
                    foreach (var oldName in oldNames)
                    {
                        System.Console.WriteLine(oldName);
                    }

                    System.Console.WriteLine();
                }
                else
                {
                    System.Console.WriteLine($"{model.Name} OK");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
