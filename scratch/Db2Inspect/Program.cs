using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Providers.WowDev.Client;

var db2Path = @"D:\TrinityCore\World of Warcraft\dbc\enUS";
var definitionsPath = @"C:\Users\mariu\Downloads\WoWDBDefs-master\WoWDBDefs-master\definitions";
var build = "12.0.1.66838";
var startLineId = 15115;

var client = new Db2Client(build, definitionsPath);

async Task<DbRow?> GetRowAsync(string db2Name, int id)
{
    var definition = await client.GetDefinitionAsync(db2Path, db2Name)
        ?? throw new InvalidOperationException($"Missing definition for {db2Name}.");

    return await client.GetSingleAsync(db2Path, db2Name, definition, new DbParameter("ID", id));
}

var visited = new HashSet<int>();
var currentLineId = startLineId;
var step = 1;

while (currentLineId > 0 && visited.Add(currentLineId))
{
    var line = await GetRowAsync("ConversationLine", currentLineId);
    if (line == null)
    {
        Console.WriteLine($"Step {step}: line {currentLineId} missing in client ConversationLine.db2");
        break;
    }

    var lineText = line.GetValueByNameAs<uint>("BroadcastTextID");
    var nextLineId = line.GetValueByNameAs<ushort>("NextConversationLineID");
    var spellVisualKitId = line.GetValueByNameAs<uint>("SpellVisualKitID");
    var additionalDuration = line.GetValueByNameAs<int>("AdditionalDuration");
    var speechType = line.GetValueByNameAs<byte>("SpeechType");
    var startAnimation = line.GetValueByNameAs<byte>("StartAnimation");
    var endAnimation = line.GetValueByNameAs<byte>("EndAnimation");

    var broadcastText = await GetRowAsync("BroadcastText", (int)lineText);

    Console.WriteLine($"Step {step}: line={currentLineId}, broadcastText={lineText}, next={nextLineId}, spellVisualKit={spellVisualKitId}, additionalDuration={additionalDuration}, speechType={speechType}, startAnim={startAnimation}, endAnim={endAnimation}");
    if (broadcastText == null)
    {
        Console.WriteLine("  BroadcastText: missing");
    }
    else
    {
        var text = broadcastText.GetValueByNameAs<string>("Text");
        var text1 = broadcastText.GetValueByNameAs<string>("Text1");
        var soundKit1 = broadcastText.GetValueByNameAs<uint>("SoundKitID1");
        var soundKit2 = broadcastText.GetValueByNameAs<uint>("SoundKitID2");
        Console.WriteLine($"  Text : {text}");
        Console.WriteLine($"  Text1: {text1}");
        Console.WriteLine($"  SoundKitID1={soundKit1}, SoundKitID2={soundKit2}");
    }

    currentLineId = nextLineId;
    step++;
    if (step > 12)
    {
        Console.WriteLine("Stopped after 12 steps.");
        break;
    }
}
