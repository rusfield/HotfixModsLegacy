/*
 * Console for debugging specific parts of the code and generating stuff.
 * These tools are not part of the compiled software.
 * 
 */

using HotfixMods.Dev.Helpers;
using HotfixMods.Providers.MySqlConnector.Client;

var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var id = await mySqlClient.GetNextIdAsync("hotfix_mods", "item_sparse", 35);


var helper = new ClientDbDefinitionHelper();
await helper.DefinitionToCSharp("AnimKit", "10.0.0.46112");