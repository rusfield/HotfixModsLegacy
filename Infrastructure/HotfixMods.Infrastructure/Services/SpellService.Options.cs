using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService
    {
        #region SpellMisc
        public async Task<Dictionary<ushort, string>> GetCastingTimeIndexOptions()
        {
            var results = new Dictionary<ushort, string>();
            var spellCastTimes = await GetAsync<SpellCastTimes>(true);
            foreach(var spellCastTime in spellCastTimes)
            {
                string value = $"{spellCastTime.Base} ms";
                if (spellCastTime.Base != spellCastTime.Minimum)
                    value += $" (min {spellCastTime.Minimum} ms)";

                results.Add((ushort)spellCastTime.ID, value);
            }
            return results;
        }

        public async Task<Dictionary<ushort, string>> GetDurationIndexOptions()
        {
            var results = new Dictionary<ushort, string>();
            var spellDurations = await GetAsync<SpellDuration>(true);
            foreach(var spellDuration in spellDurations)
            {
                string value = $"{spellDuration.Duration} ms";
                if (spellDuration.Duration != spellDuration.MaxDuration)
                    value += $" (max {spellDuration.MaxDuration} ms)";

                results.Add((ushort)spellDuration.ID,  value);
            }
            return results;
        }

        public async Task<Dictionary<ushort, string>> GetRangeIndexOptions()
        {
            var results = new Dictionary<ushort, string>();
            var spellRanges = await GetAsync<SpellRange>(true);
            foreach(var spellRange in spellRanges)
            {
                string value = $"{spellRange.RangeMin0} to {spellRange.RangeMax0} yards";
                if (spellRange.RangeMin0 != spellRange.RangeMin1 || spellRange.RangeMax0 != spellRange.RangeMax1)
                    value = $"{spellRange.RangeMin0}/{spellRange.RangeMin1} to {spellRange.RangeMax0}/{spellRange.RangeMax1} yards";

                results.Add((ushort)spellRange.ID, value);
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetSchoolMaskOptions()
        {
            return Enum.GetValues<SpellMiscSchoolMask>().ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }
        #endregion
    }
}
