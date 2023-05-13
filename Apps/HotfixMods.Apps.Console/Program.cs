// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.Listfile.Client;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.HotfixInitializer.Tool;
using HotfixMods.Tools.Initializer.Business;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HotfixMods.Core.Models;
using static DBDefsLib.Structs;
using HotfixMods.Core.Enums;

/*

var tool = new Db2ImportTool();
var db2Path = @"C:\\Program Files (x86)\\World of Warcraft\\dbc\\enUS";

foreach(var file in Directory.GetFiles(db2Path))
{
    var split = file.Split("\\");
    if (split.Length == 0)
        continue;

    var name = split.Last().Replace(".db2", "");
    try
    {
        await tool.Db2FileToDb2MySql("10.1.0.49474", db2Path, name, "HotfixMods", name, "127.0.0.1", "3306", "root", "root");
    }
    catch(Exception ex)
    {

    }

}

Console.Read();



var client = new Db2Client("10.1.0.49474");
var path = @"D:\TrinityCore\Dragonflight\dbc\enUS";
var outputPath = @"D:\TrinityCore\Customizations";
int choiceStartId = 33000;
int elementStartId = 120000;
int hotfixStartId = 901000000;
int verifiedBuild = -1340;

// Race, orderIndexFrom, orderIndexTo
List<(ChrModelId, int, int)> excluded = new() {
    //(ChrModelId.WORGEN_MALE, 0, 9999),
    //(ChrModelId.COMPANION_DRAKE, 0, 9999),
    (ChrModelId.COMPANION_PROTODRAGON, 0, 9999),
    (ChrModelId.COMPANION_PTERRODAX, 0, 9999),
    (ChrModelId.COMPANION_SERPENT, 0, 9999),
    (ChrModelId.COMPANION_WYVERN, 0, 9999),
    //ChrModelId.DRACTHYR_DRAGON, 0, 9999)
};

string choiceSql = "INSERT INTO hotfixes.chr_customization_choice values('{0}', {1}, {2}, 146, 0, {3}, {3}, 0, 90001, 0, 0, " + verifiedBuild + ");";
string elementSql = "INSERT INTO hotfixes.chr_customization_element values({0}, {1}, 0, {2}, 0, {3}, 0, 0, 0, 0, 0, " + verifiedBuild + ");";
string hotfixSql = "INSERT INTO hotfixes.hotfix_data values({0}, 0, {1}, {2}, 1, " + verifiedBuild + ");";
var choiceHash = (uint)TableHashes.CHR_CUSTOMIZATION_CHOICE;
var elementHash = (uint)TableHashes.CHR_CUSTOMIZATION_ELEMENT;

var reqDef = await client.GetDefinitionAsync(path, "ChrCustomizationReq");
var eyeOptionDef = await client.GetDefinitionAsync(path, "ChrCustomizationOption");
var eyeChoiceDef = await client.GetDefinitionAsync(path, "ChrCustomizationChoice");
var eyeElementDef = await client.GetDefinitionAsync(path, "ChrCustomizationElement");

var elementData = new Dictionary<int, List<List<(int, int)>>>(); // Dictionary<ChrModelId, List<List<(geosetId, materialId)>>>
var eyeOptions = await client.GetAsync(path, "ChrCustomizationOption", eyeOptionDef, new DbParameter("Name", "Eye Color"));
var chrCustomizationReq = (await client.GetAsync(path, "ChrCustomizationReq", reqDef));
var eyeElements = (await client.GetAsync(path, "ChrCustomizationElement", eyeElementDef));

// Step 1: Gather all data
foreach (var eyeOption in eyeOptions)
{
    var model = (ChrModelId)eyeOption.GetValueByNameAs<int>("ChrModelID");

    // Male and female share eyes. Skip female (duplicate)
    if (model.ToString().EndsWith("FEMALE"))
        continue;

    elementData[(int)model] = new();
    var eyeChoices = await client.GetAsync(path, "ChrCustomizationChoice", eyeChoiceDef, new DbParameter("ChrCustomizationOptionID", eyeOption.GetIdValue()));
    foreach (var eyeChoice in eyeChoices)
    {
        var orderIndex = eyeChoice.GetValueByNameAs<int>("OrderIndex");
        if (excluded.Any(e =>
        {
            var (modelId, from, to) = e;
            return model == modelId && orderIndex >= from && orderIndex <= to;
        }))
        {
            continue;
        }

        var eyeChoiceReqId = eyeChoice.GetValueByNameAs<int>("ChrCustomizationReqID");
        var req = chrCustomizationReq.Where(r => r.GetIdValue() == eyeChoiceReqId).FirstOrDefault();

        var classMask = req.GetValueByNameAs<int>("ClassMask");
        if (classMask == 32)
        {
            // Skip Death Knight eyes (those specifically for DK only)
            continue;
        }

        if ((req.GetValueByNameAs<int>("ReqType") & 1) != 0) // Check for flag value PLAYER
        {
            var eyeChoiceId = eyeChoice.GetIdValue();
            var choiceList = new List<(int, int)>();
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
}

// Step2: Add values across all races, excluding those that already exist on the race by default
foreach (var eyeOption in eyeOptions)
{

    var model = (ChrModelId)eyeOption.GetValueByNameAs<int>("ChrModelID");
    var modelName = model.ToDisplayString().Replace(" male", "", StringComparison.InvariantCultureIgnoreCase);

    // foreach (var elements in elementData.Where(k => k.Key != (int)model))
    foreach (var elements in elementData)
    {
        int number = 1;
        int orderIndex = 1000;
        var elementModelId = (ChrModelId)elements.Key;
        var elementModelName = elementModelId.ToDisplayString().Replace(" male", "", StringComparison.InvariantCultureIgnoreCase);
        var currentPath = Path.Combine(outputPath, $"{modelName} - {elementModelName} eyes.txt");

        using (StreamWriter sw = File.AppendText(currentPath))
        {
            if (!File.Exists(currentPath))
            {
                // Create the file

                sw.WriteLine($"Preparing {elementModelName} eyes for {modelName}");

            }

            foreach (var element in elements.Value)
            {
                var customizationName = $"{elementModelName} {number++}";
                var choiceOutput = string.Format(choiceSql, customizationName, choiceStartId, eyeOption.GetIdValue(), orderIndex, orderIndex++);
                var choiceHotfixOutput = string.Format(hotfixSql, hotfixStartId++, choiceHash, choiceStartId);

                sw.WriteLine(choiceOutput);
                sw.WriteLine(choiceHotfixOutput);
                sw.WriteLine();

                Console.WriteLine(choiceOutput);
                Console.WriteLine(choiceHotfixOutput);
                Console.WriteLine();

                foreach (var data in element)
                {
                    var (geosetId, materialId) = data;
                    var elementOutput = string.Format(elementSql, elementStartId, choiceStartId, geosetId, materialId);
                    var elementHotfixOutput = string.Format(hotfixSql, hotfixStartId++, elementHash, elementStartId++);

                    sw.WriteLine(elementOutput);
                    sw.WriteLine(elementHotfixOutput);
                    sw.WriteLine();

                    Console.WriteLine(elementOutput);
                    Console.WriteLine(elementHotfixOutput);
                    Console.WriteLine();
                }


                Console.WriteLine();
                choiceStartId++;
            }

            sw.Flush();
        }
    }
}

Console.WriteLine("Done");
Console.ReadLine();

*/

/*
var listfilePRovider = new ListfileClient();
var icons = await listfilePRovider.GetIconsAsync<int>();

foreach (var icon in icons)
    Console.WriteLine(icon.Value);

Console.WriteLine("Done");
Console.ReadLine();
*/


/*
var tool = new Db2ImportTool();
await tool.Db2FileToDb2MySql("10.0.5.47871", @"D:\TrinityCore\Dragonflight\dbc\enUS", "TransmogSet", "hotfixes", "chr_customization_req_choice", "127.0.0.1", "3306", "root", "root");
Console.Read();
*/

/*
var client = new Db2Client("10.0.5.47871");
var def = await client.GetDefinitionAsync(@"D:\TrinityCore\Dragonflight\dbc\enUS", "TransmogSet");
var data = await client.GetAsync(@"D:\TrinityCore\Dragonflight\dbc\enUS", "TransmogSet", def);

int startId = 604000;
foreach (var d in data)
{
    Console.WriteLine($"replace into transmog_set values('{d.GetValueByNameAs<string>("Name").Replace("'", "")}', {d.GetIdValue()}, 0, 0, 0, 0, 0, 0, 1, {d.GetValueByNameAs<string>("ExpansionID")}, 90205, {d.GetValueByNameAs<string>("UiOrder")}, 0, -1337);");
    //Console.WriteLine($"insert into hotfix_data values({startId++}, 0, 0x15393898, {d.GetIdValue()}, 1, -1337);");

}

Console.Read();
*/
/*
int startId = 604000;
var ids = new List<int>() { };
string query = "insert into hotfix_data values({0}, 0, 0x9B1BEE48, {1}, 1, -1337);";
foreach (var id in ids)
    Console.WriteLine(string.Format(query, startId + id, id));
Console.ReadKey();
*/
/*


var result = GetExternalClassesWithHotfixesSchema(typeof(AnimKitDto));



var reader = new TrinityCoreCodeTool();
reader.GetFields("SoulbindConduitRank");
reader.GetInstanceParameters("SoulbindConduitRank");
Console.ReadKey();
*/
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
    catch (Exception e)
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

string build = "10.1.0.49474";
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
                Console.WriteLine($"{nd.Name} ({nd.Type})");
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
Console.ReadKey();

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

//var input = "0x00000001";
//var type = typeof(uint);

T GetTValue<T>(string input)
{
    var result = (T)Convert.ChangeType(input, typeof(T));
    return result;
}