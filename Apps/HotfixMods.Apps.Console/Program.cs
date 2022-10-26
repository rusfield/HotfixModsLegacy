// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;


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
await importTool.Db2FileToDb2MySql("AnimKit", "10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "hotfix_mods", "anim_kit", "localhost", "3306", "root", "root");
Console.WriteLine("Done");
*/


var wowToolsTool = new WowToolsTool();

while (true)
{
    Console.WriteLine("Press 1 for enum and 2 for flag");
    var key = Console.ReadKey();
    try
    {
        if (key.KeyChar == '1')
            await wowToolsTool.EnumInClipboardToCSharp();
        else if (key.KeyChar == '2')
            await wowToolsTool.FlagInClipboardToCSharp();
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



