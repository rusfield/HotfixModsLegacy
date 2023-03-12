// See https://aka.ms/new-console-template for more information


using DBDefsLib;
using HotfixMods.Core.Attributes;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using HotfixMods.Tools.Dev.Business;
using HotfixMods.Tools.HotfixInitializer.Tool;
using HotfixMods.Tools.Initializer.Business;
using System.Reflection;
using System.Text.RegularExpressions;
using static DBDefsLib.Structs;

/*
var tool = new Db2ImportTool();
await tool.Db2FileToDb2MySql("10.0.5.47871", @"D:\TrinityCore\Dragonflight\dbc\enUS", "ChrCustomizationReqChoice", "hotfixes", "chr_customization_req_choice", "127.0.0.1", "3306", "root", "root");
Console.Read();
*/

int startId = 604000;
var ids = new List<int>()
{
6677,
6676,
6675,
6674,
6673,
6664,
6663,
6662,
6661,
6660,
6659,
6658,
6653,
6651,
6650,
6649,
6648,
6647,
6646,
6645,
6644,
6643,
6642,
6641,
6640,
6639,
6638,
6637,
6636,
6635,
6634,
6633,
6632,
6631,
6630,
6629,
6628,
6614,
6613,
6612,
4596,
4595,
4594,
4593,
4592,
4591,
4590,
4589,
4588,
4587,
4586,
4585,
4584,
4583,
4582,
4144,
4143,
4136,
4135,
4134,
4133,
4132,
4131,
4130,
4129,
4128,
4127,
4126,
4125,
4124,
4123,
4122,
4121,
4120,
4119,
4118,
4117,
4116,
4115,
4114,
4113,
4112,
4111,
4110,
4109,
4108,
4107,
4106,
4105,
4104,
4103,
4102,
4101,
4100,
4099,
4098,
4097,
4096,
4095,
4094,
4093,
4092,
4091,
4090,
4089,
4088,
4087,
4086,
4085,
4084,
4083,
4082,
4081,
4080,
4079,
4078,
4077,
4076,
4075,
4074,
4073,
4072,
4071,
4070,
4069,
4068,
4067,
4066,
4065,
4064,
4063,
4062,
4061,
4060,
4059,
4058,
4057,
4056,
4055,
4054,
4053,
4052,
4051,
4050,
4049,
4048,
4047,
4046,
4045,
4044,
4043,
4042,
4041,
4040,
4039,
4038,
4037,
4036,
4035,
4034,
4033,
4032,
4031,
4030,
4029,
4028,
4027,
4026,
4025,
4024,
4023,
4022,
4021,
4020,
4019,
4018,
4017,
4016,
4015,
4014,
4013,
4012,
4011,
4010,
4009,
4008,
4007,
4006,
4005,
4004,
4003,
4002,
4001,
4000,
3999,
3998,
3997,
3996,
3995,
3994,
3993,
3992,
3991,
3990,
3989,
3988,
3987,
3986,
3985,
3984,
3983,
3982,
3981,
3980,
3979,
3978,
3977,
3976,
3975,
3974,
3973,
3972,
3971,
3970,
3969,
3968,
3967,
3966,
3965,
3964,
3963,
3962,
3961,
3960,
3959,
3958,
3957,
3956,
3955,
3954,
3953,
3952,
3951,
3950,
3949,
3948,
3947,
3946,
3945,
3944,
3943,
3942,
3941,
3940,
3939,
3938,
3937,
3936,
3935,
3934,
3933,
3932,
3931,
3930,
3929,
3928,
3927,
3926,
3925,
3924,
3923,
3922,
3921,
3920,
3919,
3918,
3917,
3916,
3915,
3914,
3913,
3912,
3911,
3910,
3909,
3908,
3907,
3906,
3905,
3904,
3903,
3902,
3901,
3900,
3899,
3898,
3897,
3896,
3895,
3894,
3893,
3892,
3891,
3890,
3889,
3888,
3887,
3886,
3885,
3884,
3883,
3882,
3881,
3880,
3879,
3878,
3877,
3876,
3875,
3874,
3873,
3872,
3871,
3870,
3869,
3868,
3867,
3866,
3865,
3864,
3863,
3862,
3861,
3860,
3859,
3858,
3857,
3856,
3855,
3854,
3853,
3852,
3851,
3850,
3849,
3848,
3847,
3846,
3845,
3844,
3843,
3842,
3841,
3840,
3839,
3838,
3837,
3836,
3835,
3834,
3833,
3832,
3831,
3830,
3829,
3828,
3827,
3826,
3825,
3824,
3823,
3822,
3821,
3820,
3819,
3818,
3817,
3816,
3815,
3814,
3813,
3812,
3811,
3810,
3809,
3808,
3807,
3806,
3805,
3804,
3803,
3802,
3801,
3800,
3799,
3798,
3797,
3796,
3795,
3794,
3793,
3792,
3791,
3790,
3789,
3788,
3787,
3786,
3785,
3784,
3783,
3782,
3781,
3780,
3779,
3778,
3777,
3776,
3775,
3774,
3773,
3772,
3771,
3770,
3769,
3768,
3767,
3766,
3765,
3764,
3763,
3762,
3761,
3760,
3759,
3758,
3757,
3756,
3755,
3754,
3753,
3752,
3751,
3750,
3749,
3748,
3747,
3746,
3745,
3744,
3743,
3742,
3741,
3740,
3739,
3738,
3737,
3736,
3735,
3734,
3733,
3732,
3731,
3730,
3729,
3728,
3727,
3726,
3725,
3724,
3723,
3722,
3721,
3720,
3719,
3718,
3717,
3716,
3715,
3714,
3713,
3712,
3711,
3710,
3709,
3708,
3707,
3706,
3705,
3704,
3703,
3702,
3701,
3700,
3699,
3698,
3697,
3696,
3695,
3694,
3693,
3692
};
string query = "insert into hotfix_data values({0}, 0, 0x9B1BEE48, {1}, 1, -1337);";
foreach (var id in ids)
    Console.WriteLine(string.Format(query, startId + id, id));
Console.ReadKey();

/*


var result = GetExternalClassesWithHotfixesSchema(typeof(AnimKitDto));



var reader = new TrinityCoreCodeTool();
reader.GetFields("SoulbindConduitRank");
reader.GetInstanceParameters("SoulbindConduitRank");
Console.ReadKey();
*/
/*
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");


var serverDef = await serverDefHelper.GetDefinitionAsync("characters", "character_customizations");


var tctool = new TrinityCoreDbTool();
await tctool.DbDefToCSharp(serverDef);
Console.ReadKey();
*/

/*
int totalInvokes = 4;
int current = 1;
Func<int> exp = () => current++ * 100 / totalInvokes;
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.WriteLine(exp());
Console.ReadKey();
*/

/*
var hotfixTool = new HotfixTableTool();
var db2Types = new List<Type>
{
    typeof(AnimKitSegment)
};
hotfixTool.GenerateAll(db2Types.ToArray());
Console.ReadKey();
*/

/*
var tcTool = new TrinityCoreDbTool();
await tcTool.Db2HashEnumInClipboardToCSharp();
Console.ReadKey();
*/

/*
var mySqlClient = new MySqlClient("localhost", "3306", "root", "root");
var definition = await mySqlClient.GetDefinitionAsync("world", "screen_effect");
var tcdTool = new TrinityCoreDbTool();
await tcdTool.DbDefToCSharp(definition);
Console.ReadKey();
*/



var dt = new DbDefinitionTool();
string build = "10.0.5.47871";
while (true)
{
    Console.WriteLine("Enter db2 name:");
    var line = Console.ReadLine();
    try
    {
        await dt.DefinitionToCSharp(line, build);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    Console.ReadKey();
    Console.Clear();
}







/*
var importTool = new Db2ImportTool();
await importTool.Db2FileToDb2MySql("10.0.2.46157", "C:\\Users\\Disconnected\\Downloads", "spellmisc", "hotfix_mods", "spell_misc", "localhost", "3306", "root", "root");
Console.WriteLine("Done");
*/




/*

var wowToolsTool = new WowToolsTool();

while (true)
{
    Console.WriteLine("Press 1 for enum or 2 for flag or 3 for array");
    var key = Console.ReadKey();
    try
    {
        if (key.KeyChar == '1')
            await wowToolsTool.EnumInClipboardToCSharp();
        else if (key.KeyChar == '2')
            await wowToolsTool.FlagInClipboardToCSharp();
        else if (key.KeyChar == '3')
            await wowToolsTool.ArrayInClipboardToCSharp();
        else
            Console.WriteLine("Invalid key.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    Console.ReadKey();
    Console.Clear();
}
*/

/*
// Compare client (master) and server definition
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");
string build = "10.0.2.46924";
var clientDefHelper = new Db2Client(build);

var serverDef = await serverDefHelper.GetDefinitionAsync("hotfixes", "creature_display_info_option");
var clientDef = await clientDefHelper.GetDefinitionAsync(@"C:\hotfixMods", "creaturedisplayinfooption");
var tool = new DbDefinitionTool();
tool.CompareDefinitions(clientDef, serverDef);
Console.ReadKey();
*/


/*
// Compare property names to db definition
var serverDefHelper = new MySqlClient("localhost", "3306", "root", "root");
var serverAssembly = Assembly.Load("HotfixMods.Core");

var serverModels = serverAssembly.GetTypes().Where(t => t.Namespace == "HotfixMods.Core.Models.TrinityCore").ToList();
foreach (var model in serverModels)
{
    Console.WriteLine($"Checking {model.Name}");
    var properties = model.GetProperties().Select(p => p.Name).ToList();

    string schema = "";
    if (model.GetCustomAttribute(typeof(HotfixesSchemaAttribute)) != null)
        schema = "hotfixes";

    if (model.GetCustomAttribute(typeof(WorldSchemaAttribute)) != null)
        schema = "world";

    if (model.GetCustomAttribute(typeof(HotfixModsSchemaAttribute)) != null)
        schema = "hotfixMods";

    if (model.GetCustomAttribute(typeof(CharactersSchemaAttribute)) != null)
        schema = "characters";

    if(schema == "")
    {
        Console.WriteLine("No schema found for " + model.Name);
        continue;
    }

    var definition = await serverDefHelper.GetDefinitionAsync(schema, model.Name.ToTableName());
    if(null == definition)
    {
        Console.WriteLine("No definitions found for " + model.Name);
        continue;
    }
    var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
    var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

    foreach (var prop in model.GetProperties())
    {
        var defProp = definition.ColumnDefinitions.Where(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        if (defProp == null)
        {
            Console.WriteLine($"{prop.Name} not found");
            continue;
        }
        else if (prop.PropertyType != defProp.Type)
        {
            Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
            Console.ReadKey();
        }
    }

    if (oldNames.Any() || newDefinitions.Any())
    {
        Console.Clear();
        Console.WriteLine($"Updated values for {model.Name}");
        Console.WriteLine("New names:");
        foreach (var nd in newDefinitions)
        {
            Console.WriteLine($"{nd.Name}");
        }
        Console.WriteLine("Old names:");
        foreach (var on in oldNames)
        {
            Console.WriteLine(on);
        }
        Console.ReadKey();
        Console.Clear();
    }
    else
    {
        Console.WriteLine($"{model.Name} OK");
    }
}
*/


// Compare property names to definition names
/*
string build = "10.0.5.47660";
var defHelper = new Db2Client(build);
var assembly = Assembly.Load("HotfixMods.Core");
var models = assembly.GetTypes().Where(t => t.Namespace == "HotfixMods.Core.Models.Db2").ToList();

bool skip = true;
foreach (var model in models)
{
    var properties = model.GetProperties().Select(p => p.Name).ToList();
    try
    {
        Console.WriteLine($"Checking {model.Name}");
        var definition = await defHelper.GetDefinitionAsync(null, model.Name);
        var newDefinitions = definition.ColumnDefinitions.Where(d => !properties.Any(p => p.Equals(d.Name, StringComparison.InvariantCultureIgnoreCase)));
        var oldNames = properties.Where(p => !definition.ColumnDefinitions.Any(d => d.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

        foreach(var prop in model.GetProperties())
        {
            var defProp = definition.ColumnDefinitions.Where(d => d.Name.Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if(defProp == null)
            {
                Console.WriteLine($"{prop.Name} not found");
                continue;
            }
            else if(prop.PropertyType != defProp.Type)
            {
                Console.WriteLine($"Property mismatch on {prop.Name}. {prop.PropertyType} should be {defProp.Type}");
                Console.ReadKey();
            }
        }

        if (oldNames.Any() || newDefinitions.Any())
        {
            Console.Clear();
            Console.WriteLine($"Updated values for {model.Name}");
            Console.WriteLine("New names:");
            foreach (var nd in newDefinitions)
            {
                Console.WriteLine($"{nd.Name}");
            }
            Console.WriteLine("Old names:");
            foreach (var on in oldNames)
            {
                Console.WriteLine(on);
            }
            Console.ReadKey();
            Console.Clear();
        }
        else
        {
            Console.WriteLine($"{model.Name} OK");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
*/

/*


// Generate InfoModel
Console.WriteLine("Enter name of DTO ( ex. CreatureDto )");
var input = Console.ReadLine();
var assembly = Assembly.Load("HotfixMods.Infrastructure");
var dto = assembly.GetType($"HotfixMods.Infrastructure.DtoModels.{input}");
if (dto== null)
{
    Console.WriteLine("DTO not found");
    Console.ReadKey();
}
else
{
    var properties = dto.GetProperties();

    foreach (var property in properties)
    {
        var type = property.PropertyType;
        if (type.IsGenericType)
        {
            type = type.GetGenericArguments()[0];
        }
        if (!type.IsValueType)
        {
            Console.WriteLine("namespace HotfixMods.Infrastructure.InfoModels");
            Console.WriteLine("{");
            Console.WriteLine($"public class {type.Name}Info : IInfoModel");
            Console.WriteLine("{");
            var db2Properties = type.GetProperties();
            foreach (var db2Property in db2Properties)
            {
                if (db2Property.Name == "VerifiedBuild" || db2Property.Name == "Id")
                    continue;
                Console.WriteLine($"public string {db2Property.Name}" + " { get; set; } = \"TODO\";");
            }
            Console.WriteLine();
            Console.WriteLine("public string ModelInfo { get; set; } = \"TODO\";");
            Console.WriteLine("public bool IsRequired { get; set; } = false;");
            Console.WriteLine("}");
            Console.WriteLine("}");
        }
        Console.ReadKey();
        Console.Clear();
    }
}
Console.WriteLine("Done");
Console.ReadKey();
*/

List<Type> GetExternalClassesWithHotfixesSchema(Type type)
{
    var externalClasses = new List<Type>();

    foreach (var property in type.GetProperties())
    {
        var propertyType = property.PropertyType;

        if (propertyType == typeof(List<>))
        {
            propertyType = propertyType.GetGenericArguments()[0];
        }

        if (!propertyType.IsPrimitive && propertyType.Namespace != "System")
        {
            if (propertyType.GetCustomAttribute<HotfixesSchemaAttribute>() != null)
            {
                externalClasses.Add(propertyType);
            }

            externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(propertyType));
        }
    }

    foreach (var innerType in type.GetNestedTypes())
    {
        if (innerType.IsClass && innerType.IsNestedPublic)
        {
            externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(innerType));
        }
    }

    return externalClasses.Distinct().ToList();
}