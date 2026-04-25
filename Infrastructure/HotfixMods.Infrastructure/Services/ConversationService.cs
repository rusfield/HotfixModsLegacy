using HotfixMods.Core.Interfaces;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class ConversationService : ServiceBase
    {
        public ConversationService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.ConversationSettings.FromId;
            ToId = appConfig.ConversationSettings.ToId;
            VerifiedBuild = appConfig.ConversationSettings.VerifiedBuild;
        }

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var entities = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                return entities
                    .Select(e => new DashboardModel()
                    {
                        ID = e.RecordID,
                        Name = e.Name,
                        AvatarUrl = null
                    })
                    .OrderByDescending(e => e.ID)
                    .ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new();
        }

        public async Task<ConversationDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(8);

            try
            {
                var conversationTemplate = await GetSingleAsync<ConversationTemplate>(callback, progress, new DbParameter(nameof(ConversationTemplate.Id), id));
                if (conversationTemplate == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(ConversationTemplate)} not found", 100);
                    return null;
                }

                var result = new ConversationDto()
                {
                    ConversationTemplate = conversationTemplate,
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, id),
                    IsUpdate = true
                };

                var actors = await GetAsync<ConversationActors>(callback, progress, new DbParameter(nameof(ConversationActors.ConversationId), id));
                foreach (var actor in actors.OrderBy(a => a.Idx))
                {
                    result.ActorGroups.Add(new()
                    {
                        ConversationActors = actor
                    });
                }

                var visitedLineIds = new HashSet<int>();
                var nextLineId = conversationTemplate.FirstLineId;
                while (nextLineId > 0 && visitedLineIds.Add(nextLineId))
                {
                    var lineTemplate = await GetSingleAsync<ConversationLineTemplate>(callback, progress, new DbParameter(nameof(ConversationLineTemplate.Id), nextLineId));
                    var conversationLine = await GetSingleAsync<ConversationLine>(callback, progress, new DbParameter(nameof(ConversationLine.ID), nextLineId));
                    if (lineTemplate == null || conversationLine == null)
                    {
                        break;
                    }

                    var broadcastText = await GetSingleAsync<BroadcastText>(callback, progress, new DbParameter(nameof(BroadcastText.ID), conversationLine.BroadcastTextID))
                        ?? new BroadcastText() { ID = conversationLine.BroadcastTextID };

                    result.LineGroups.Add(new()
                    {
                        ConversationLineTemplate = lineTemplate,
                        ConversationLine = conversationLine,
                        BroadcastText = broadcastText
                    });

                    nextLineId = conversationLine.NextConversationLineID;
                }

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = GetConversationDisplayName(result);
                }

                callback.Invoke(LoadingHelper.Loading, "Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return null;
        }

        public async Task<bool> SaveAsync(ConversationDto dto, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(8);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.ConversationTemplate.Id);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.ConversationTemplate);
                await SaveAsync(callback, progress, dto.ActorGroups.Select(g => g.ConversationActors).ToList());
                await SaveAsync(callback, progress, dto.LineGroups.Select(g => g.ConversationLineTemplate).ToList());
                await SaveAsync(callback, progress, dto.LineGroups.Select(g => g.BroadcastText).ToList());
                await SaveAsync(callback, progress, dto.LineGroups.Select(g => g.ConversationLine).ToList());

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                dto.HotfixModsEntity.Name = GetConversationDisplayName(dto);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback ??= DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(10);

            try
            {
                var dto = await GetByIdAsync(id);
                if (dto == null)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                foreach (var lineGroup in dto.LineGroups)
                {
                    if (lineGroup.BroadcastText.ID > 0)
                    {
                        await DeleteAsync(callback, progress, lineGroup.BroadcastText);
                    }

                    if (lineGroup.ConversationLine.ID > 0)
                    {
                        await DeleteAsync(callback, progress, lineGroup.ConversationLine);
                    }

                    if (HasConfiguredVerifiedBuild(lineGroup.ConversationLineTemplate))
                    {
                        await DeleteAsync(callback, progress, lineGroup.ConversationLineTemplate);
                    }
                }

                foreach (var actorGroup in dto.ActorGroups.Where(a => HasConfiguredVerifiedBuild(a.ConversationActors)))
                {
                    await DeleteAsync(
                        callback,
                        progress,
                        _appConfig.WorldSchema,
                        nameof(ConversationActors).ToTableName(),
                        new DbParameter(nameof(ConversationActors.ConversationId), actorGroup.ConversationActors.ConversationId),
                        new DbParameter(nameof(ConversationActors.Idx), actorGroup.ConversationActors.Idx));
                }

                if (HasConfiguredVerifiedBuild(dto.ConversationTemplate))
                {
                    await DeleteAsync(callback, progress, dto.ConversationTemplate);
                }

                await DeleteAsync(callback, progress, dto.HotfixModsEntity);

                callback.Invoke(LoadingHelper.Deleting, "Delete successful", 100);
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }

            return false;
        }

        string GetConversationDisplayName(ConversationDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.HotfixModsEntity.Name))
            {
                return dto.HotfixModsEntity.Name;
            }

            var lineText = dto.LineGroups
                .Select(g => GetPreferredBroadcastText(g.BroadcastText))
                .FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            if (!string.IsNullOrWhiteSpace(lineText))
            {
                return lineText;
            }

            return dto.ConversationTemplate.Id > 0 ? $"Conversation {dto.ConversationTemplate.Id}" : "New Conversation";
        }

        static string GetPreferredBroadcastText(BroadcastText? broadcastText)
        {
            if (!string.IsNullOrWhiteSpace(broadcastText?.Text1))
            {
                return broadcastText.Text1;
            }

            return broadcastText?.Text ?? "";
        }
    }
}
