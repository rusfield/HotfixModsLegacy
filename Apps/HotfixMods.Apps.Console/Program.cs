// See https://aka.ms/new-console-template for more information


using HotfixMods.Tools.Initializer.Business;

var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("AnimKit", "10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "hotfix_mods", "anim_kit", "localhost", "3306", "root", "root");
Console.WriteLine("Done");