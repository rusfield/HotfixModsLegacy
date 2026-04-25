using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ConversationService
    {
        async Task SetIdAndVerifiedBuild(ConversationDto dto)
        {
            var isCreateConversationTemplate = IsCreateOperation(dto.IsUpdate, dto.ConversationTemplate.Id);

            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var conversationId = await GetIdByConditionsAsync<ConversationTemplate>(dto.ConversationTemplate.Id, dto.IsUpdate);
            var nextConversationLineId = await GetNextConversationLineIdAsync();
            var nextBroadcastTextId = await GetNextBroadcastTextIdAsync();

            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = conversationId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.ConversationTemplate.Id = conversationId;
            SetConfiguredVerifiedBuildOnCreate(dto.ConversationTemplate, isCreateConversationTemplate);

            for (int i = 0; i < dto.ActorGroups.Count; i++)
            {
                var actor = dto.ActorGroups[i].ConversationActors;
                var isCreateActor = IsCreateOperation(dto.IsUpdate, actor.ConversationId);

                actor.ConversationId = conversationId;
                actor.Idx = (ushort)i;
                SetConfiguredVerifiedBuildOnCreate(actor, isCreateActor);
            }

            for (int i = 0; i < dto.LineGroups.Count; i++)
            {
                var lineGroup = dto.LineGroups[i];
                var isCreateLine = IsCreateOperation(dto.IsUpdate, lineGroup.ConversationLineTemplate.Id);
                var isCreateBroadcastText = IsCreateOperation(dto.IsUpdate, lineGroup.BroadcastText.ID);
                var lineId = isCreateLine
                    ? nextConversationLineId++
                    : lineGroup.ConversationLineTemplate.Id;
                var broadcastTextId = isCreateBroadcastText
                    ? nextBroadcastTextId++
                    : lineGroup.BroadcastText.ID;

                if (lineId > ushort.MaxValue)
                {
                    throw new Exception("Unable to allocate a conversation line ID within the supported range.");
                }

                lineGroup.ConversationLineTemplate.Id = lineId;
                SetConfiguredVerifiedBuildOnCreate(lineGroup.ConversationLineTemplate, isCreateLine);

                lineGroup.ConversationLine.ID = lineId;
                lineGroup.ConversationLine.BroadcastTextID = broadcastTextId;
                lineGroup.ConversationLine.NextConversationLineID = 0;
                lineGroup.ConversationLine.VerifiedBuild = VerifiedBuild;

                lineGroup.BroadcastText.ID = broadcastTextId;
                lineGroup.BroadcastText.VerifiedBuild = VerifiedBuild;
            }

            for (int i = 0; i < dto.LineGroups.Count - 1; i++)
            {
                dto.LineGroups[i].ConversationLine.NextConversationLineID = (ushort)dto.LineGroups[i + 1].ConversationLine.ID;
            }

            dto.ConversationTemplate.FirstLineId = dto.LineGroups.FirstOrDefault()?.ConversationLineTemplate.Id ?? 0;
            dto.HotfixModsEntity.Name = GetConversationDisplayName(dto);
        }

        async Task<int> GetNextConversationLineIdAsync()
        {
            var nextWorldId = await GetNextIdInRangeAsync(_appConfig.WorldSchema, nameof(ConversationLineTemplate).ToTableName(), 1, ushort.MaxValue, nameof(ConversationLineTemplate.Id));
            var nextHotfixId = await GetNextIdInRangeAsync(_appConfig.HotfixesSchema, nameof(ConversationLine).ToTableName(), 1, ushort.MaxValue, nameof(ConversationLine.ID));

            var nextId = Math.Max(nextWorldId, nextHotfixId);
            if (nextId > ushort.MaxValue)
            {
                throw new Exception("No more conversation line IDs are available below 65535.");
            }

            return nextId;
        }

        async Task<int> GetNextBroadcastTextIdAsync()
        {
            return await GetNextIdInRangeAsync(_appConfig.HotfixesSchema, nameof(BroadcastText).ToTableName(), FromId, ToId, nameof(BroadcastText.ID));
        }
    }
}
