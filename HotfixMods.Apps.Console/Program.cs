/*
 * 
 * Playground
 * 
 */


using HotfixMods.Db2Provider.WowToolsFiles.Clients;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Services;
using HotfixMods.MySqlProvider.EntityFrameworkCore.Clients;
using HotfixMods.Core.Enums;
using HotfixMods.Apps.Console;
using HotfixMods.Core.Constants;
using System.Text;

foreach (var enumValue in Enum.GetValues(typeof(DB2Hash)))
{
    var enumString = enumValue.ToString();
    enumString = enumString.Replace("_", "");
    enumString = AddSpacesToSentence(enumString, true);
    Console.WriteLine($"{enumString.ToUpper()} = {(uint)enumValue},");
}


string AddSpacesToSentence(string text, bool preserveAcronyms)
{
    if (string.IsNullOrWhiteSpace(text))
        return string.Empty;
    StringBuilder newText = new StringBuilder(text.Length * 2);
    newText.Append(text[0]);
    for (int i = 1; i < text.Length; i++)
    {
        if (char.IsUpper(text[i]))
            if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                 i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                newText.Append('_');
        newText.Append(text[i]);
    }
    return newText.ToString();
}