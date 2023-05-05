using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Helpers;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<ushort, string>> GetFactionOptionsAsync()
        {
            return await GetFactionOptionsAsync<ushort>();
        }

        public async Task<Dictionary<ulong, string>> GetMechanicImmuneMaskOptionsAsync()
        {
            var mechanics = await GetAsync(_appConfig.HotfixesSchema, "SpellMechanic", false, true);
            var results = new Dictionary<ulong, string>();
            results.Add(0, "None");
            ulong keyIndex = 1;
            foreach (var mechanic in mechanics)
            {
                results[keyIndex] = $"{mechanic.GetValueByNameAs<string>("StateName").ToDisplayString()}";
                keyIndex *= 2;
            }
            return results;
        }

        #region CreatureTemplate
        public async Task<Dictionary<string, string>> GetAiNameOptionsAsync()
        {
            return Enum.GetValues<CreatureTemplateAiNames>().ToDictionary(key => key.ToString(), value => value.ToString());
        }

        public async Task<Dictionary<string, string>> GetIconNameOptionsAsync()
        {
            return Enum.GetValues<IconNames>().ToDictionary(key => key.ToString(), value => value.ToString());
        }
        #endregion

        public async Task<Dictionary<uint, string>> GetFlagsExtraOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Flags_Extra));
        }

        public async Task<Dictionary<uint, string>> GetUnitFlagsOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Unit_Flags));
        }

        public async Task<Dictionary<uint, string>> GetUnitFlags2OptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Unit_Flags2));
        }

        public async Task<Dictionary<uint, string>> GetUnitFlags3OptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Unit_Flags3));
        }

        public async Task<Dictionary<byte, string>> GetTypeOptionsAsync()
        {
            return await GetDb2OptionsAsync<byte>("CreatureType", "Name");
        }

        public async Task<Dictionary<int, string>> GetFamilyOptionsAsync()
        {
            var options = await GetDb2OptionsAsync<int>("CreatureFamily", "Name");
            if (!options.ContainsKey(0))
            {
                options[0] = "None";
                options = options.SortByKey();
            }
            return options;
        }

        public async Task<Dictionary<byte, string>> GetRankOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(CreatureTemplate), nameof(CreatureTemplate.Rank));
        }

        public async Task<Dictionary<byte, string>> GetTrainerClassOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(CreatureTemplate), nameof(CreatureTemplate.Trainer_Class));
        }

        public async Task<Dictionary<byte, string>> GetMovementTypeOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(CreatureTemplate), nameof(CreatureTemplate.MovementType));
        }

        public async Task<Dictionary<byte, string>> GetRacialLeaderOptionsAsync()
        {
            return new Dictionary<byte, string>() {
                { 0, "False" },
                { 1, "True" }
            };
        }

        public async Task<Dictionary<byte, string>> GetRegenHealthOptionsAsync()
        {
            return new Dictionary<byte, string>() {
                { 0, "False" },
                { 1, "True" }
            };
        }

        public async Task<Dictionary<sbyte, string>> GetDmgschoolOptionsAsync()
        {
            return await GetEnumOptionsAsync<sbyte>(typeof(CreatureTemplate), nameof(CreatureTemplate.Dmgschool));
        }

        public async Task<Dictionary<uint, string>> GetSpellSchoolImmuneMaskOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Spell_School_Immune_Mask));
        }

        public async Task<Dictionary<uint, string>> GetTypeFlagsOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Type_Flags));
        }

        public async Task<Dictionary<uint, string>> GetTypeFlags2OptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.Type_Flags2));
        }

        public async Task<Dictionary<uint, string>> GetDynamicFlagsOptionsAsync()
        {
            return await GetEnumOptionsAsync<uint>(typeof(CreatureTemplate), nameof(CreatureTemplate.DynamicFlags));
        }

        public async Task<Dictionary<ulong, string>> GetNpcFlagOptionsAsync()
        {
            return await GetEnumOptionsAsync<ulong>(typeof(CreatureTemplate), nameof(CreatureTemplate.NpcFlag));
        }

        public async Task<Dictionary<int, string>> GetRequiredExpansionOptionsAsync()
        {
            return await GetEnumOptionsAsync<int>(typeof(CreatureTemplate), nameof(CreatureTemplate.HealthScalingExpansion));
        }

        public async Task<Dictionary<int, string>> GetHealthScalingExpansionOptionsAsync()
        {
            return await GetEnumOptionsAsync<int>(typeof(CreatureTemplate), nameof(CreatureTemplate.HealthScalingExpansion));
        }

        public async Task<Dictionary<byte, string>> GetUnitClassOptionsAsync()
        {
            return await GetEnumOptionsAsync<byte>(typeof(CreatureTemplate), nameof(CreatureTemplate.Unit_Class));
        }

        public async Task<Dictionary<int, string>> GetVignetteIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<int>("Vignette", "Name");
        }

        #region CreatureDisplayInfo
        public async Task<Dictionary<ushort, string>> GetParticleColorIdOptionsAsync()
        {
            return await GetParticleColorOptionsAsync<ushort>();
        }

        public async Task<Dictionary<sbyte, string>> GetGenderOptionsAsync()
        {
            return Enum.GetValues<Gender>().ToDictionary(key => (sbyte)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<ushort, string>> GetModelIdOptionsAsync()
        {
            return await GetCreatureModelDataOptionsAsync<ushort>();
        }
        #endregion

        #region CreatureDisplayInfoExtra
        public async Task<Dictionary<sbyte, string>> GetDisplaySexIdOptionsAsync()
        {
            return Enum.GetValues<Gender>().ToDictionary(key => (sbyte)key, value => value.ToDisplayString());
        }

        public async Task<Dictionary<sbyte, string>> GetDisplayClassIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<sbyte>("ChrClasses", "Name");
        }

        public async Task<Dictionary<sbyte, string>> GetDisplayRaceIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<sbyte>("ChrRaces", "Name");
        }
        #endregion
    }
}
