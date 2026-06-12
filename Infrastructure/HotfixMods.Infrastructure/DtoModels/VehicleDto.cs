using HotfixMods.Core.Models.Db2;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class VehicleDto : DtoBase
    {
        public VehicleDto() : base("Vehicle") { }

        public Vehicle Vehicle { get; set; } = new();

        public List<VehicleSeatGroup> VehicleSeatGroups { get; set; } = new();
        public class VehicleSeatGroup
        {
            public VehicleSeat VehicleSeat { get; set; } = new();
        }
    }
}
