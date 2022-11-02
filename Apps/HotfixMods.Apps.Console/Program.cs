// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;



float myFloat = 123;
decimal myDecimal = Convert.ToDecimal(myFloat);

var t1 = myFloat.GetType();
var t2 = myDecimal.GetType();

var myClass = new MyClass();
var myProperty = myClass.GetType().GetProperty("MyProperty");
myProperty.SetValue(myClass, myDecimal);

Console.ReadKey();

public class MyClass
{
    public decimal MyProperty { get; set; }
}


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
*/



