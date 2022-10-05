/*
 * Used during development.
 * Not part of public software.
 */


using HotfixMods.Providers.DbDef.WoWDev.Client;
using HotfixMods.Dev.Helpers;
using HotfixMods.Providers.MySql.MySqlConnector.Client;

// var helper = new WowToolsConverter();
// helper.ConvertFlagToCSharp(@"C:\Users\Disconnected\Desktop\flagstest.txt");

/*
for (long i = 1; i <= 67108864; i = i * 2)
{
    Console.WriteLine($"UNK_{i} = {i},");
}
*/

var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var defClient = new DbDefClient();

var builds = await defClient.GetAvailableBuildsForDefinitionAsync("ItemSparse");
var build = builds.Last();

var def = await defClient.GetAvailableColumnsAsync("ItemSparse", build);

await mySqlClient.CreateTableIfNotExistAsync("hotfix_mods", "item_sparse", def);
await mySqlClient.CreateTableIfNotExistAsync("hotfix_mods3", "item_sparse", def);