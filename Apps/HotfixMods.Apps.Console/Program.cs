﻿// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;
using System.Reflection;
using static DBDefsLib.Structs;



/*
var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var definition = await mySqlClient.GetDefinitionAsync("world", "creature_model_info");
var tcdTool = new TrinityCoreDbTool();
await tcdTool.DbDefToCSharp(definition);
Console.ReadKey();
*/


/*
var dt = new DbDefinitionTool();
string build = "10.0.2.46924";
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


// Compare property names to definition names
string build = "10.0.2.46924";
var defHelper = new Db2Client(build);
var assembly = Assembly.Load("HotfixMods.Core");
var models = assembly.GetTypes().Where(t => t.Namespace == "HotfixMods.Core.Models.Db2").ToList();

bool skip = true;
foreach (var model in models)
{
    var properties = model.GetProperties().Select(p => p.Name).ToList();
    try
    {
        // Use this to continue after reaching GitHub anonymous limitation (or just get a token)
        /*
        if(model.Name  == "SpellVisual")
        {
            skip = false;
        }
        if (skip)
        {
            continue;
        }
        */

        var definition = await defHelper.GetDefinitionAsync(null, model.Name);
        var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
        var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

        if(oldNames.Any() || newDefinitions.Any())
        {
            Console.Clear();
            Console.WriteLine($"Updated values for {model.Name}");
            Console.WriteLine("New names:");
            foreach(var nd in newDefinitions)
            {
                Console.WriteLine($"{nd.Name}");
            }
            Console.WriteLine("Old names:");
            foreach(var on in oldNames)
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

