using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.AggregateModels;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Handlers;
using HotfixMods.Infrastructure.Helpers;
using HotfixMods.Providers.Interfaces;
using HotfixMods.Providers.Models;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SoundKitService : ServiceBase
    {
        public SoundKitService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerValuesProvider serverValuesProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverValuesProvider, listfileProvider, exceptionHandler, appConfig){}

        public async Task<List<DashboardModel>> GetDashboardModelsAsync()
        {
            try
            {
                var dtos = await GetAsync<HotfixModsEntity>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(HotfixData.VerifiedBuild), VerifiedBuild));
                var results = new List<DashboardModel>();
                foreach (var dto in dtos)
                {
                    results.Add(new()
                    {
                        ID = dto.RecordID,
                        Name = dto.Name,
                        AvatarUrl = null
                    });
                }
                return results.OrderByDescending(d => d.ID).ToList();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<SoundKitDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var soundKit = await    GetSingleAsync<SoundKit>(callback, progress, new DbParameter(nameof(SoundKit.ID), id));
                if (null == soundKit)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(SoundKit)} not found", 100);
                    return null;
                }

                var result = new SoundKitDto()
                {
                    SoundKit = soundKit,
                    EntryGroups = new(),
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, soundKit.ID),
                    IsUpdate = true
                };

                var soundKitEntries = await GetAsync<SoundKitEntry>(callback, progress, new DbParameter(nameof(SoundKitEntry.SoundKitID), id));
                foreach (var soundKitEntry in soundKitEntries)
                {
                    result.EntryGroups.Add(new()
                    {
                        SoundKitEntry = soundKitEntry
                    });
                }

                callback.Invoke(LoadingHelper.Loading, $"Loading successful", 100);
                return result;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
            }
            return null;
        }

        public async Task<bool> SaveAsync(SoundKitDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                await SetIdAndVerifiedBuild(dto);

                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.SoundKit.ID);
                }

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.SoundKit);
                await SaveAsync(callback, progress, dto.EntryGroups.Select(s => s.SoundKitEntry).ToList());

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
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
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(4);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                await DeleteAsync(callback, progress, dto.EntryGroups.Select(s => s.SoundKitEntry).ToList());
                await DeleteAsync(callback, progress, dto.SoundKit);
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
    }
}
