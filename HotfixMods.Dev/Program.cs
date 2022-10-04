/*
 * Used during development.
 * Not part of public software.
 */


using HotfixMods.DbDefProvider.WoWDev.Client;
using HotfixMods.Dev.Helpers;

// var helper = new WowToolsConverter();
// helper.ConvertFlagToCSharp(@"C:\Users\Disconnected\Desktop\flagstest.txt");

/*
for (long i = 1; i <= 67108864; i = i * 2)
{
    Console.WriteLine($"UNK_{i} = {i},");
}
*/

var defProvider = new DbDefClient();

var builds = await defProvider.GetAvailableBuildsForDefinitionAsync("ItemSparse.db2");
foreach(var build in builds)
{
    Console.WriteLine(build);
}

var definitions = await defProvider.GetAvailableDefinitionsAsync();
foreach (var definition in definitions)
{
    Console.WriteLine(definition);
}

var content = await defProvider.GetAvailableColumnsAsync("ItemSparse", "10.0.0.45697");
foreach(var c in content)
{
    Console.WriteLine($"{c.Value.ToString()}\t{c.Key}");
}

