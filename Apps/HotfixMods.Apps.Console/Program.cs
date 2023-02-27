// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Core.Attributes;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;
using System.Reflection;
using System.Text.RegularExpressions;
using static DBDefsLib.Structs;



var result = GetExternalClassesWithHotfixesSchema(typeof(AnimKitDto));



var reader = new TrinityCoreCodeTool();
reader.GetFields("SoulbindConduitRank");
reader.GetInstanceParameters("SoulbindConduitRank");
Console.ReadKey();

/*
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");


var serverDef = await serverDefHelper.GetDefinitionAsync("characters", "character_customizations");


var tctool = new TrinityCoreDbTool();
await tctool.DbDefToCSharp(serverDef);
Console.ReadKey();
*/

/*
int totalInvokes = 4;
int current = 1;
Func<int> exp = () => current++ * 100 / totalInvokes;
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.ReadKey();
*/

/*
var hotfixTool = new HotfixTableTool();
var db2Types = new List<Type>
{
    typeof(AnimKitSegment)
};
hotfixTool.GenerateAll(db2Types.ToArray());
Console.ReadKey();
*/

/*
var tcTool = new TrinityCoreDbTool();
await tcTool.Db2HashEnumInClipboardToCSharp();
Console.ReadKey();
*/

/*
var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var definition = await mySqlClient.GetDefinitionAsync("world", "screen_effect");
var tcdTool = new TrinityCoreDbTool();
await tcdTool.DbDefToCSharp(definition);
Console.ReadKey();
*/


/*
var dt = new DbDefinitionTool();
string build = "10.0.5.47871";
while (true)
{
    Console.WriteLine("Enter db2 name:");
    var line = Console.ReadLine();
    try
    {
        await dt.DefinitionToCSharp(line, build);
    }
    catch(Exception e)
    {
        Console.WriteLine(e.Message);
    }
    Console.ReadKey();
    Console.Clear();
}
*/






/*
var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "spellmisc", "hotfix_mods", "spell_misc", "localhost", "3306", "root", "root");
Console.WriteLine("Done");
*/




/*

var wowToolsTool = new WowToolsTool();

while (true)
{
    Console.WriteLine("Press 1 for enum or 2 for flag or 3 for array");
    var key = Console.ReadKey();
    try
    {
        if (key.KeyChar == '1')
            await wowToolsTool.EnumInClipboardToCSharp();
        else if (key.KeyChar == '2')
            await wowToolsTool.FlagInClipboardToCSharp();
        else if (key.KeyChar == '3')
            await wowToolsTool.ArrayInClipboardToCSharp();
        else
            Console.WriteLine("Invalid key.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    Console.ReadKey();
    Console.Clear();
}
*/

/*
// Compare client (master) and server definition
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");
string build = "10.0.2.46924";
var clientDefHelper = new Db2Client(build);

var serverDef = await serverDefHelper.GetDefinitionAsync("hotfixes", "creature_display_info_option");
var clientDef = await clientDefHelper.GetDefinitionAsync(@"C:\hotfixMods", "creaturedisplayinfooption");
var tool = new DbDefinitionTool();
tool.CompareDefinitions(clientDef, serverDef);
Console.ReadKey();
*/


/*
// Compare property names to db definition
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");
var serverAssembly = Assembly.Load("HotfixMods.Core");

var serverModels = serverAssembly.GetTypes().Where(t => t.Namespace == "HotfixMods.Core.Models.TrinityCore").ToList();
foreach (var model in serverModels)
{
    Console.WriteLine($"Checking {model.Name}");
    var properties = model.GetProperties().Select(p => p.Name).ToList();

    string schema = "";
    if (model.GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
        schema = "hotfixes";

    if (model.GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
        schema = "world";

    if (model.GetCustomAttribute(typeof(HotfixModsSchemaAttribute)) != null)
        schema = "hotfixMods";

    if (model.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
        schema = "characters";

    if(schema == "")
    {
        Console.WriteLine("No schema found for " + model.Name);
        continue;
    }

    var definition = await serverDefHelper.GetDefinitionAsync(schema, model.Name.ToTableName());
    if(null == definition)
    {
        Console.WriteLine("No definitions found for " + model.Name);
        continue;
    }
    var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
    var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

    foreach (var prop in model.GetProperties())
    {
        var defProp = definition.ColumnDefinitions.Where(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        if (defProp == null)
        {
            Console.WriteLine($"{prop.Name} not found");
            continue;
        }
        else if (prop.PropertyType != defProp.Type)
        {
            Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
            Console.ReadKey();
        }
    }

    if (oldNames.Any() || newDefinitions.Any())
    {
        Console.Clear();
        Console.WriteLine($"Updated values for {model.Name}");
        Console.WriteLine("New names:");
        foreach (var nd in newDefinitions)
        {
            Console.WriteLine($"{nd.Name}");
        }
        Console.WriteLine("Old names:");
        foreach (var on in oldNames)
        {
            Console.WriteLine(on);
        }
        Console.ReadKey();
        Console.Clear();
    }
    else
    {
        Console.WriteLine($"{model.Name} OK");
    }
}
*/


// Compare property names to definition names
/*
string build = "10.0.5.47660";
var defHelper = new Db2Client(build);
var assembly = Assembly.Load("HotfixMods.Core");
var models = assembly.GetTypes().Where(t => t.Namespace == "HotfixMods.Core.Models.Db2").ToList();

bool skip = true;
foreach (var model in models)
{
    var properties = model.GetProperties().Select(p => p.Name).ToList();
    try
    {
        Console.WriteLine($"Checking {model.Name}");
        var definition = await defHelper.GetDefinitionAsync(null, model.Name);
        var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
        var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

        foreach(var prop in model.GetProperties())
        {
            var defProp = definition.ColumnDefinitions.Where(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if(defProp == null)
            {
                Console.WriteLine($"{prop.Name} not found");
                continue;
            }
            else if(prop.PropertyType != defProp.Type)
            {
                Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
                Console.ReadKey();
            }
        }

        if (oldNames.Any() || newDefinitions.Any())
        {
            Console.Clear();
            Console.WriteLine($"Updated values for {model.Name}");
            Console.WriteLine("New names:");
            foreach (var nd in newDefinitions)
            {
                Console.WriteLine($"{nd.Name}");
            }
            Console.WriteLine("Old names:");
            foreach (var on in oldNames)
            {
                Console.WriteLine(on);
            }
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"{model.Name} OK");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
*/

/*


// Generate InfoModel
Console.WriteLine("Enter name of DTO ( ex. CreatureDto )");
var input = Console.ReadLine();
var assembly = Assembly.Load("HotfixMods.Infrastructure");
var dto = assembly.GetType($"HotfixMods.Infrastructure.DtoModels.{input}");
if (dto== null)
{
    Console.WriteLine("DTO not found");
    Console.ReadKey();
}
else
{
    var properties = dto.GetProperties();

    foreach (var property in properties)
    {
        var type = property.PropertyType;
        if (type.IsGenericType)
        {
            type = type.GetGenericArguments()[0];
        }
        if (!type.IsValueType)
        {
            Console.WriteLine("namespace HotfixMods.Infrastructure.InfoModels");
            Console.WriteLine("{");
            Console.WriteLine($"public class {type.Name}Info : IInfoModel");
            Console.WriteLine("{");
            var db2Properties = type.GetProperties();
            foreach (var db2Property in db2Properties)
            {
                if (db2Property.Name == "VerifiedBuild" || db2Property.Name == "Id")
                    continue;
                Console.WriteLine($"public string {db2Property.Name}" + " { get; set; } = \"TODO\";");
            }
            Console.WriteLine();
            Console.WriteLine("public string ModelInfo { get; set; } = \"TODO\";");
            Console.WriteLine("public bool IsRequired { get; set; } = false;");
            Console.WriteLine("}");
            Console.WriteLine("}");
        }
        Console.ReadKey();
        Console.Clear();
    }
}
Console.WriteLine("Done");
Console.ReadKey();
*/

List<Type> GetExternalClassesWithHotfixesSchema(Type type)
{
    var externalClasses = new List<Type>();

    foreach (var property in type.GetProperties())
    {
        var propertyType = property.PropertyType;

        if (propertyType == typeof(List<>))
        {
            propertyType = propertyType.GetGenericArguments()[0];
        }

        if (!propertyType.IsPrimitive && propertyType.Namespace != "System")
        {
            if (propertyType.GetCustomAttribute<HotfixesSchemaAttribute>() != null)
            {
                externalClasses.Add(propertyType);
            }

            externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(propertyType));
        }
    }

    foreach (var innerType in type.GetNestedTypes())
    {
        if (innerType.IsClass && innerType.IsNestedPublic)
        {
            externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(innerType));
        }
    }

    return externalClasses.Distinct().ToList();
}