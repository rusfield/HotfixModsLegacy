/*
 * Console for debugging specific parts of the code and generating stuff.
 * These tools are not part of the compiled software.
 * 
 */

using HotfixMods.Dev.Helpers;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using System.Net;

var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var definitionClient = new Db2Client("10.0.0.46112");

var def = await definitionClient.GetDefinitionAsync("C:\\Users\\Disconnected\\Downloads", "ItemSparse");
var data = await definitionClient.GetAsync("C:\\Users\\Disconnected\\Downloads", "ItemSparse", def);



await mySqlClient.AddOrUpdateAsync("hotfix_mods", "item_sparse", data.ToArray());





await mySqlClient.CreateTableIfNotExistsAsync("hotfix_mods", "item_sparse", await definitionClient.GetDefinitionAsync(null, "ItemSparse"));


var id = await mySqlClient.GetNextIdAsync("hotfix_mods", "item_sparse", 35);


var helper = new ClientDbDefinitionHelper();
await helper.DefinitionToCSharp("AnimKit", "10.0.0.46112");