/*
 * Console for debugging specific parts of the code and generating stuff.
 * These tools are not part of the compiled software.
 * 
 */


var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("AnimKit", "10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "hotfix_mods", "localhost", "3306", "root", "root");
Console.WriteLine("Done");