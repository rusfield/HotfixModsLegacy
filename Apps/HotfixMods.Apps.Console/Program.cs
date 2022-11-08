// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;




var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var definition = await mySqlClient.GetDefinitionAsync("world", "gameobject_template_addon");
var tcdTool = new TrinityCoreDbTool();
await tcdTool.DbDefToCSharp(definition);
Console.ReadKey();

/*
var dt = new DbDefinitionTool();
string build = "10.0.2.46157";
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
await importTool.Db2FileToDb2MySql("10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "ItemSearchName", "hotfix_mods", "item_search_name", "localhost", "3306", "root", "root");
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
