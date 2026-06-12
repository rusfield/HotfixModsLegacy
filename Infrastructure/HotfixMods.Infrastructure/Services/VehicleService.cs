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
    public partial class VehicleService : ServiceBase
    {
        int VehicleSeatFromId;
        int VehicleSeatToId;

        public VehicleService(IServerDbDefinitionProvider serverDbDefinitionProvider, IClientDbDefinitionProvider clientDbDefinitionProvider, IServerDbProvider serverDbProvider, IClientDbProvider clientDbProvider, IServerEnumProvider serverEnumProvider, IListfileProvider listfileProvider, IExceptionHandler exceptionHandler, AppConfig appConfig)
            : base(serverDbDefinitionProvider, clientDbDefinitionProvider, serverDbProvider, clientDbProvider, serverEnumProvider, listfileProvider, exceptionHandler, appConfig)
        {
            FromId = appConfig.VehicleSettings.FromId;
            ToId = appConfig.VehicleSettings.ToId;
            VerifiedBuild = appConfig.VehicleSettings.VerifiedBuild;
            VehicleSeatFromId = appConfig.VehicleSeatSettings.FromId;
            VehicleSeatToId = appConfig.VehicleSeatSettings.ToId;
        }

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

        public async Task<VehicleDto?> GetByIdAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(6);

            try
            {
                var vehicle = await GetSingleAsync<Vehicle>(callback, progress, new DbParameter(nameof(Vehicle.ID), id));
                if (vehicle == null)
                {
                    callback.Invoke(LoadingHelper.Loading, $"{nameof(Vehicle)} not found", 100);
                    return null;
                }

                var result = new VehicleDto()
                {
                    HotfixModsEntity = await GetExistingOrNewHotfixModsEntityAsync(callback, progress, id),
                    Vehicle = vehicle,
                    VehicleSeatGroups = new(),
                    IsUpdate = true
                };

                var seatIds = new ushort[] { vehicle.SeatID0, vehicle.SeatID1, vehicle.SeatID2, vehicle.SeatID3, vehicle.SeatID4, vehicle.SeatID5, vehicle.SeatID6, vehicle.SeatID7 };
                foreach (var seatId in seatIds.Where(s => s > 0))
                {
                    var seat = await GetSingleAsync<VehicleSeat>(callback, progress, new DbParameter(nameof(VehicleSeat.ID), (int)seatId));
                    if (seat != null)
                    {
                        result.VehicleSeatGroups.Add(new() { VehicleSeat = seat });
                    }
                }

                if (string.IsNullOrWhiteSpace(result.HotfixModsEntity.Name))
                {
                    result.HotfixModsEntity.Name = $"Vehicle {id}";
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

        public async Task<bool> SaveAsync(VehicleDto dto, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(6);

            try
            {
                callback.Invoke(LoadingHelper.Saving, "Deleting existing data", progress());
                if (dto.IsUpdate)
                {
                    await DeleteAsync(dto.Vehicle.ID);
                }

                callback.Invoke(LoadingHelper.Saving, "Preparing to save", progress());
                await SetIdAndVerifiedBuild(dto);

                await SaveAsync(callback, progress, dto.HotfixModsEntity);
                await SaveAsync(callback, progress, dto.Vehicle);
                await SaveAsync(callback, progress, dto.VehicleSeatGroups.Select(g => g.VehicleSeat).ToList());

                callback.Invoke(LoadingHelper.Saving, "Saving successful", 100);
                dto.IsUpdate = true;
                return true;
            }
            catch (Exception ex)
            {
                callback.Invoke("Error", ex.Message, 100);
                HandleException(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, Action<string, string, int>? callback = null)
        {
            callback = callback ?? DefaultCallback;
            var progress = LoadingHelper.GetLoaderFunc(5);

            try
            {
                var dto = await GetByIdAsync(id);
                if (null == dto)
                {
                    callback.Invoke(LoadingHelper.Deleting, "Nothing to delete", 100);
                    return false;
                }

                foreach (var group in dto.VehicleSeatGroups)
                {
                    await DeleteAsync(group.VehicleSeat);
                }

                await DeleteAsync(callback, progress, dto.Vehicle);
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
