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

Console.WriteLine("Hello, World!");

var service = new ItemService(
    new Db2Client(),
    new MySqlClient(
        "127.0.0.1",
        "root",
        "root",
        "world",
        "characters",
        "hotfixes"),
    -1200,
    10000000,
    99999999);

var item = new ItemDto()
{
    Id = 5000600,
    IconId = 135031,
    ComponentArmUpper = 526357,
    ComponentArmLower = 526356,
    Bonding = ItemBondings.NOT_BOUND,
    OverallQuality = OverallQualities.UNCOMMON,
    RequiredLevel = 0,
    Name = "Creation Legs 2",
    ItemLevel = 1,
    Flags1 = (ItemFlags1)8192,
    Flags2 = (ItemFlags2)8388608,
    Flags3 = (ItemFlags3)2,
    Material = ItemMaterial.LEATHER,
    ModelResourceId0 = 51474,
    ModelMaterialResourceId0 = 526358
};

await service.SaveItemAsync(item);