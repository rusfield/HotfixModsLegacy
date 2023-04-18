using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Flags.Db2;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Infrastructure.Extensions;

namespace HotfixMods.Infrastructure.Services
{
    public partial class SpellService
    {
        #region SpellMisc
        public async Task<Dictionary<ushort, string>> GetCastingTimeIndexOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var spellCastTimes = await GetAsync(_appConfig.HotfixesSchema, "SpellCastTimes", false, true);
            foreach(var spellCastTime in spellCastTimes)
            {
                var spellCastTimeBase = spellCastTime.GetValueByNameAs<int>("Base");
                var spellCastTimeMinimum = spellCastTime.GetValueByNameAs<int>("Minimum");

                var value = $"{spellCastTimeBase} ms";
                if (spellCastTimeBase != spellCastTimeMinimum)
                    value += $" (min {spellCastTimeMinimum} ms)";

                results.Add((ushort)spellCastTime.GetIdValue(), value);
            }
            return results;
        }

        public async Task<Dictionary<ushort, string>> GetDurationIndexOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var spellDurations = await GetAsync(_appConfig.HotfixesSchema, "SpellDuration", false, true);
            foreach (var spellDuration in spellDurations)
            {
                var duration = spellDuration.GetValueByNameAs<int>("Duration");
                var maxDuration = spellDuration.GetValueByNameAs<int>("MaxDuration");

                var value = $"{duration} ms";
                if (duration != maxDuration)
                    value += $" (min {maxDuration} ms)";

                results.Add((ushort)spellDuration.GetIdValue(), value);
            }
            return results;
        }

        public async Task<Dictionary<ushort, string>> GetRangeIndexOptionsAsync()
        {
            var results = new Dictionary<ushort, string>();
            var spellDurations = await GetAsync(_appConfig.HotfixesSchema, "SpellRange", false, true);
            foreach (var spellDuration in spellDurations)
            {
                var rangeMin0 = spellDuration.GetValueByNameAs<decimal>("RangeMin0");
                var rangeMin1 = spellDuration.GetValueByNameAs<decimal>("RangeMin1");
                var rangeMax0 = spellDuration.GetValueByNameAs<decimal>("RangeMax0");
                var rangeMax1 = spellDuration.GetValueByNameAs<decimal>("RangeMax1");

                string value = $"{rangeMin0} to {rangeMax0} yards";
                if(rangeMin0 != rangeMin1 || rangeMax0 != rangeMax1)
                    value = $"{rangeMin0}/{rangeMin1} to {rangeMax0}/{rangeMax1} yards";

                results.Add((ushort)spellDuration.GetIdValue(), value);
            }
            return results;
        }

        public async Task<Dictionary<byte, string>> GetSchoolMaskOptions()
        {
            return Enum.GetValues<SpellMiscSchoolMask>().ToDictionary(key => (byte)key, value => value.ToDisplayString());
        }
        #endregion

        #region SpellAuraOptions
        public async Task<Dictionary<ushort, string>> GetSpellProcsPerMinuteIdOptionsAsync()
        {
            var options = await GetDb2OptionsAsync<ushort>("SpellProcsPerMinute", "BaseProcRate");
            foreach(var option in options.Where(o => o.Key != 0))
            {
                options[option.Key] = $"Rate: {options[option.Key]}";
            }
            return options;
        }

        public async Task<Dictionary<byte, string>> GetDifficultyIdOptionsAsync()
        {
            return await GetDifficultyOptionsAsync<byte>();
        }
        #endregion

        #region SpellPower
        public async Task<Dictionary<sbyte, string>> GetPowerTypeOptionsAsync()
        {
            var options = await GetEnumOptionsAsync<sbyte>(typeof(SpellPower), nameof(SpellPower.PowerType));
            return options.SortByKey();
        }

        public async Task<Dictionary<uint, string>> GetPowerDisplayIdOptionsAsync()
        {
            var options = await GetDb2OptionsAsync<uint>("PowerDisplay", "GlobalStringBaseTag");
            foreach(var option in options)
                options[option.Key] = option.Value.ToDisplayString();

            return options;
        }

        public async Task<Dictionary<int, string>> GetAltPowerBarIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("UnitPowerBar", "Name");
        }

        public async Task<Dictionary<int, string>> GetRequiredAuraSpellIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("SpellName", "Name");
        }
        #endregion
    }
}
