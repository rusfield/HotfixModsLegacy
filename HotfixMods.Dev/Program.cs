/*
 * Used during development.
 * Not part of public software.
 */


using HotfixMods.Dev.Helpers;

// var helper = new WowToolsConverter();
// helper.ConvertFlagToCSharp(@"C:\Users\Disconnected\Desktop\flagstest.txt");

for (long i = 1; i <= 67108864; i = i * 2)
{
    Console.WriteLine($"UNK_{i} = {i},");
}