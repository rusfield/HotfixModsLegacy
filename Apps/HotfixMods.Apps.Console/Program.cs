// See https://aka.ms/new-console-template for more information


using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;



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

/*
var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("AnimKit", "10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "hotfix_mods", "anim_kit", "localhost", "3306", "root", "root");
Console.WriteLine("Done");


var wowToolsTool = new WowToolsTool();
var output = await wowToolsTool.EnumInClipboardToCSharp();
*/





