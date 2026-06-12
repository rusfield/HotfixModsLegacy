using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class QuestService
    {
        async Task SetIdAndVerifiedBuild(QuestDto dto)
        {
            var isCreateQuestTemplate = IsCreateOperation(dto.IsUpdate, (int)dto.QuestTemplate.ID);

            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var questTemplateId = await GetIdByConditionsAsync<QuestTemplate>((int)dto.QuestTemplate.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextQuestObjectivesId = await GetNextIdAsync<QuestObjectives>();
            var nextQuestObjectiveId = await GetNextIdAsync<QuestObjective>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = questTemplateId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.QuestTemplate.ID = (uint)questTemplateId;
            SetConfiguredVerifiedBuildOnCreate(dto.QuestTemplate, isCreateQuestTemplate);

            dto.QuestOfferReward.ID = (uint)questTemplateId;
            SetConfiguredVerifiedBuildOnCreate(dto.QuestOfferReward, isCreateQuestTemplate);

            dto.QuestRequestItems.ID = (uint)questTemplateId;
            SetConfiguredVerifiedBuildOnCreate(dto.QuestRequestItems, isCreateQuestTemplate);

            dto.QuestV2.ID = (uint)questTemplateId;
            dto.QuestV2.VerifiedBuild = VerifiedBuild;

            if (dto.QuestTemplateAddon != null)
            {
                dto.QuestTemplateAddon.ID = (uint)questTemplateId;
            }

            if (dto.QuestDetails != null)
            {
                dto.QuestDetails.ID = (uint)questTemplateId;
                SetConfiguredVerifiedBuildOnCreate(dto.QuestDetails, isCreateQuestTemplate);
            }

            foreach (var group in dto.QuestObjectiveGroups)
            {
                group.QuestObjectives.ID = (uint)nextQuestObjectivesId;
                group.QuestObjectives.QuestID = (uint)questTemplateId;
                group.QuestObjectives.VerifiedBuild = VerifiedBuild;

                group.QuestObjective.ID = nextQuestObjectivesId;
                group.QuestObjective.QuestID = questTemplateId;
                group.QuestObjective.VerifiedBuild = VerifiedBuild;

                nextQuestObjectivesId++;
                nextQuestObjectiveId++;
            }

            foreach (var group in dto.CreatureQueststarterGroups)
            {
                group.CreatureQueststarter.Quest = (uint)questTemplateId;
            }

            foreach (var group in dto.GameobjectQueststarterGroups)
            {
                group.GameobjectQueststarter.Quest = (uint)questTemplateId;
            }

            foreach (var group in dto.GameobjectQuestenderGroups)
            {
                group.GameobjectQuestender.Quest = (uint)questTemplateId;
            }
        }
    }
}
