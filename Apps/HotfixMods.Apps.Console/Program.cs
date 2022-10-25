// See https://aka.ms/new-console-template for more information


using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.Initializer.Business;



var class1 = new TestClass() { MyProperty= 1 };
var class2 = new TestClass() { MyProperty= 2 };

var struct1 = new MyStruct<TestClass>();
struct1.T1 = class1;

var struct2 = struct1;
struct1.T2 = class2;

Console.WriteLine(struct2.T2.MyProperty);




/*
var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("AnimKit", "10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "hotfix_mods", "anim_kit", "localhost", "3306", "root", "root");
Console.WriteLine("Done");
*/

var wowToolsTool = new WowToolsTool();
var output = await wowToolsTool.EnumInClipboardToCSharp();









public class TestClass
{
    public int MyProperty {  get; set; }
}

public struct MyStruct<T>
{
    public T T1 { get; set; }
    public T T2 { get; set; }
}