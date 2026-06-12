using HotfixMods.Core.Flags.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class VehicleService
    {
        public async Task<Dictionary<int, string>> GetSeatFlagsOptionsAsync()
        {
            return Enum.GetValues<VehicleSeatFlags>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<int, string>> GetSeatFlagsBOptionsAsync()
        {
            return Enum.GetValues<VehicleSeatFlagsB>().ToDictionary(key => (int)key, value => value.ToDisplayString());
        }
    }
}
