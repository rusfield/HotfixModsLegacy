using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Services
{
    public partial class VehicleService
    {
        async Task SetIdAndVerifiedBuild(VehicleDto dto)
        {
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var vehicleId = await GetIdByConditionsAsync<Vehicle>(dto.Vehicle.ID, dto.IsUpdate);
            var nextVehicleSeatId = await GetNextIdInRangeAsync(_appConfig.HotfixesSchema, "vehicle_seat", VehicleSeatFromId, VehicleSeatToId, "ID");

            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = vehicleId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.Vehicle.ID = vehicleId;
            dto.Vehicle.VerifiedBuild = VerifiedBuild;

            for (int i = 0; i < dto.VehicleSeatGroups.Count && i < 8; i++)
            {
                var seat = dto.VehicleSeatGroups[i].VehicleSeat;
                seat.ID = nextVehicleSeatId++;
                seat.VerifiedBuild = VerifiedBuild;
                var seatId = (ushort)seat.ID;
                switch (i)
                {
                    case 0: dto.Vehicle.SeatID0 = seatId; break;
                    case 1: dto.Vehicle.SeatID1 = seatId; break;
                    case 2: dto.Vehicle.SeatID2 = seatId; break;
                    case 3: dto.Vehicle.SeatID3 = seatId; break;
                    case 4: dto.Vehicle.SeatID4 = seatId; break;
                    case 5: dto.Vehicle.SeatID5 = seatId; break;
                    case 6: dto.Vehicle.SeatID6 = seatId; break;
                    case 7: dto.Vehicle.SeatID7 = seatId; break;
                }
            }
        }
    }
}
