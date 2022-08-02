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

/*
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
    Id = 10010020,
    IconId = 132692,
    Bonding = ItemBondings.NOT_BOUND,
    OverallQuality = OverallQualities.UNCOMMON,
    RequiredLevel = 0,
    Name = "Skirt and Robes Test",
    //ComponentTorsoUpper = 44871,
    //ComponentTorsoLower = 45378,
    //ComponentLegUpper = 80193,
    //ComponentLegLower = 80192,
    ItemLevel = 1,
    Flags1 = (ItemFlags1)8192,
    Flags2 = (ItemFlags2)8388608,
    Flags3 = (ItemFlags3)2,
    Material = ItemMaterial.LEATHER,
    ModelResourceId0 = 51474,
    ModelMaterialResourceId0 = 526358
};

await service.SaveItemAsync(item);

*/