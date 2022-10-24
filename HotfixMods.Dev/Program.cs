/*
 * Used during development.
 * Not part of public software.
 */


using HotfixMods.Core.Attributes;
using HotfixMods.Dev.Helpers;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using System.ComponentModel.DataAnnotations.Schema;



// var helper = new WowToolsConverter();
// helper.ConvertFlagToCSharp(@"C:\Users\Disconnected\Desktop\flagstest.txt");




//await DefinitionHelper.DefinitionToCSharp("ItemSparse", "10.0.0.46112");


var itemSparse = new ItemSparse()
{
    IDYp = 123
};

var id = itemSparse.GetId();


var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var defClient = new Db2Client("10.0.0.46112");

var builds = await defClient.GetAvailableBuildsForDefinitionAsync("ItemSparse");
var build = builds.First();



var def = await defClient.GetDefinitionAsync("C:\\Users\\Disconnected\\Downloads", "ItemSparse");
var data = await defClient.GetAsync("C:\\Users\\Disconnected\\Downloads", "ItemSparse", def);



await mySqlClient.AddOrUpdateAsync("hotfix_mods", "item_sparse", data.ToArray());

Console.WriteLine("Done");
/*
var def = await defClient.GetAvailableColumnsAsync("ItemSparse", build);

await mySqlClient.CreateTableIfNotExistAsync("hotfix_mods", "item_sparse", def);

var values = new Dictionary<string, KeyValuePair<Type, object?>>();
foreach (var d in def)
{
    values.Add(d.Key, new(d.Value, null));
}

await mySqlClient.AddOrUpdateAsync("hotfix_mods", "item_sparse", values);

var vals = await mySqlClient.GetAsync<ItemSparse>("hotfix_mods", "item_sparse", "id = 0");
Console.WriteLine(vals.First().ID);
*/




public class ItemSparse
{
    [Id]
    public int IDYp { get; set; }
    public long AllowableRace { get; set; }
    public string Description { get; set; }
    public string Display3 { get; set; }
    public string Display2 { get; set; }
    public string Display1 { get; set; }
    public string Display { get; set; }
    public int ExpansionID { get; set; }
    public decimal DmgVariance { get; set; }
    public int LimitCategory { get; set; }
    public uint DurationInInventory { get; set; }
    public decimal QualityModifier { get; set; }
    public uint BagFamily { get; set; }
    public int StartQuestID { get; set; }
    public int LanguageID { get; set; }
    public decimal ItemRange { get; set; }
    public decimal StatPercentageOfSocket1 { get; set; }
    public decimal StatPercentageOfSocket2 { get; set; }
    public decimal StatPercentageOfSocket3 { get; set; }
    public decimal StatPercentageOfSocket4 { get; set; }
    public decimal StatPercentageOfSocket5 { get; set; }
    public decimal StatPercentageOfSocket6 { get; set; }
    public decimal StatPercentageOfSocket7 { get; set; }
    public decimal StatPercentageOfSocket8 { get; set; }
    public decimal StatPercentageOfSocket9 { get; set; }
    public decimal StatPercentageOfSocket10 { get; set; }
    public int StatPercentEditor1 { get; set; }
    public int StatPercentEditor2 { get; set; }
    public int StatPercentEditor3 { get; set; }
    public int StatPercentEditor4 { get; set; }
    public int StatPercentEditor5 { get; set; }
    public int StatPercentEditor6 { get; set; }
    public int StatPercentEditor7 { get; set; }
    public int StatPercentEditor8 { get; set; }
    public int StatPercentEditor9 { get; set; }
    public int StatPercentEditor10 { get; set; }
    public int Stackable { get; set; }
    public int MaxCount { get; set; }
    public int MinReputation { get; set; }
    public uint RequiredAbility { get; set; }
    public uint SellPrice { get; set; }
    public uint BuyPrice { get; set; }
    public uint VendorStackCount { get; set; }
    public decimal PriceVariance { get; set; }
    public decimal PriceRandomValue { get; set; }
    public int Flags1 { get; set; }
    public int Flags2 { get; set; }
    public int Flags3 { get; set; }
    public int Flags4 { get; set; }
    public int OppositeFactionItemID { get; set; }
    public int ModifiedCraftingReagentItemID { get; set; }
    public int ContentTuningID { get; set; }
    public int PlayerLevelToItemLevelCurveID { get; set; }
    public ushort ItemNameDescriptionID { get; set; }
    public ushort RequiredTransmogHoliday { get; set; }
    public ushort RequiredHoliday { get; set; }
    public ushort Gem_properties { get; set; }
    public ushort Socket_match_enchantment_ID { get; set; }
    public ushort TotemCategoryID { get; set; }
    public ushort InstanceBound { get; set; }
    public ushort ZoneBound1 { get; set; }
    public ushort ZoneBound2 { get; set; }
    public ushort ItemSet { get; set; }
    public ushort LockID { get; set; }
    public ushort PageID { get; set; }
    public ushort ItemDelay { get; set; }
    public ushort MinFactionID { get; set; }
    public ushort RequiredSkillRank { get; set; }
    public ushort RequiredSkill { get; set; }
    public ushort ItemLevel { get; set; }
    public short AllowableClass { get; set; }
    public byte ArtifactID { get; set; }
    public byte SpellWeight { get; set; }
    public byte SpellWeightCategory { get; set; }
    public byte SocketType1 { get; set; }
    public byte SocketType2 { get; set; }
    public byte SocketType3 { get; set; }
    public byte SheatheType { get; set; }
    public byte Material { get; set; }
    public byte PageMaterialID { get; set; }
    public byte Bonding { get; set; }
    public byte DamageType { get; set; }
    public sbyte StatModifier_bonusStat1 { get; set; }
    public sbyte StatModifier_bonusStat2 { get; set; }
    public sbyte StatModifier_bonusStat3 { get; set; }
    public sbyte StatModifier_bonusStat4 { get; set; }
    public sbyte StatModifier_bonusStat5 { get; set; }
    public sbyte StatModifier_bonusStat6 { get; set; }
    public sbyte StatModifier_bonusStat7 { get; set; }
    public sbyte StatModifier_bonusStat8 { get; set; }
    public sbyte StatModifier_bonusStat9 { get; set; }
    public sbyte StatModifier_bonusStat10 { get; set; }
    public byte ContainerSlots { get; set; }
    public byte RequiredPVPMedal { get; set; }
    public byte RequiredPVPRank { get; set; }
    public sbyte RequiredLevel { get; set; }
    public sbyte InventoryType { get; set; }
    public sbyte OverallQualityID { get; set; }
    public int VerifiedBuild { get; set; }
}