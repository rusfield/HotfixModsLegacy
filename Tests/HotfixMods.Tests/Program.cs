using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Tools.Dev.Business;

var dto = new GossipDto();
var creatureDto = new CreatureDto();

Assert(dto.MenuGroups.Count == 0, "GossipDto should start with no menu groups.");
Assert(creatureDto.CreatureTemplateGossip == null, "CreatureDto should expose creature_template_gossip as an optional link.");

dto.MenuGroups.Add(new GossipDto.MenuGroup());
dto.MenuGroups[0].GreetingTextGroups.Add(new GossipDto.GreetingTextGroup());
dto.OptionGroups.Add(new GossipDto.OptionGroup());

Assert(dto.MenuGroups[0].GossipMenu is GossipMenu, "Menu group must expose gossip_menu.");
Assert(dto.MenuGroups[0].NpcText is NpcText, "Menu group must expose npc_text.");
Assert(dto.MenuGroups[0].GreetingTextGroups[0].BroadcastText is BroadcastText, "Greeting text group must expose broadcast_text.");
Assert(dto.OptionGroups[0].GossipMenuOption is GossipMenuOption, "Option group must expose gossip_menu_option.");
Assert(dto.OptionGroups[0].BroadcastText is BroadcastText, "Option group must expose option broadcast_text.");
Assert(dto.OptionGroups[0].GossipNpcOption is GossipNpcOption, "Option group must expose gossip_npc_option.");

var script = CustomizationRequirementUnlockTool.GenerateScript(
    [
        new CustomizationRequirementUnlockRow(
            ID: 12,
            ReqType: 2,
            RaceMask: -1,
            ClassMask: 0,
            RegionGroupMask: 7,
            ReqAchievementID: 123,
            ReqQuestID: 456,
            OverrideArchive: -1,
            ReqItemModifiedAppearanceID: 789,
            ReqSource: "Curious source")
    ],
    new CustomizationRequirementUnlockOptions
    {
        HotfixStartId = 902100000,
        VerifiedBuild = -55500
    });

Assert(script.Contains("SET @VerifiedBuild = -55500;"), "Customization unlock script should declare the configured negative VerifiedBuild.");
Assert(script.Contains("REPLACE INTO hotfixes.chr_customization_req"), "Customization unlock script should write chr_customization_req rows.");
Assert(script.Contains("VALUES (12, -1, 'Curious source', 3, 0, 7, 0, 0, -1, 0, @VerifiedBuild);"), "Customization unlock script should preserve masks/source/archive, clear unlock IDs, and convert NPC req type to choice req type.");
Assert(script.Contains("INSERT INTO hotfixes.hotfix_data (Id, UniqueId, TableHash, RecordId, Status, VerifiedBuild) VALUES (902100000, 0, 1631787621, 12, 1, @VerifiedBuild);"), "Customization unlock script should write matching hotfix_data rows.");

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception(message);
    }
}
