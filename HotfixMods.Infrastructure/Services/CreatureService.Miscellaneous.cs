using HotfixMods.Core.Enums;
using HotfixMods.Core.Models;
using HotfixMods.Infrastructure.DtoModels.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        public async Task<Dictionary<CustomizationOptionDto, List<CustomizationChoiceDto>>> GetAvailableCustomizations(Races race, Genders gender, bool includeDruidForms = false, Action<string, string, int>? progressCallback = null)
        {
            if (progressCallback == null)
                progressCallback = ConsoleProgressCallback;

            var result = new Dictionary<CustomizationOptionDto, List<CustomizationChoiceDto>>();
            var chrModel = ConvertRaceAndGenderToChrModel(race, gender);
            if (chrModel == null)
                return result;

            var options = await _db2.GetManyAsync<ChrCustomizationOption>(c => c.ChrModelId == chrModel);
            foreach (var option in options)
            {
                if (!includeDruidForms && IsDruidFormCustomization(option))
                    continue;

                var choices = (await _db2.GetManyAsync<ChrCustomizationChoice>(c => c.ChrCustomizationOptionId == option.Id)).ToList();
                result.Add(new CustomizationOptionDto()
                {
                    Id = option.Id,
                    Name = option.Name
                },
                (from a in choices
                 select new CustomizationChoiceDto()
                 {
                     Id = a.Id,
                     Name = $"{choices.FindIndex(c => c.Id == a.Id) + 1} {a.Name}"
                 }).ToList()
                );
            }
            return result;
        }

        public ChrModels? ConvertRaceAndGenderToChrModel(Races race, Genders gender)
        {
            return (race, gender) switch
            {
                (Races.HUMAN, Genders.Male) => ChrModels.HUMAN_MALE,
                (Races.HUMAN, Genders.Female) => ChrModels.HUMAN_FEMALE,
                (Races.ORC, Genders.Male) => ChrModels.ORC_MALE,
                (Races.ORC, Genders.Female) => ChrModels.ORC_FEMALE,
                (Races.DWARF, Genders.Male) => ChrModels.DWARF_MALE,
                (Races.DWARF, Genders.Female) => ChrModels.DWARF_FEMALE,
                (Races.NIGHT_ELF, Genders.Male) => ChrModels.NIGHT_ELF_MALE,
                (Races.NIGHT_ELF, Genders.Female) => ChrModels.NIGHT_ELF_FEMALE,
                (Races.UNDEAD, Genders.Male) => ChrModels.UNDEAD_MALE,
                (Races.UNDEAD, Genders.Female) => ChrModels.UNDEAD_FEMALE,
                (Races.TAUREN, Genders.Male) => ChrModels.TAUREN_MALE,
                (Races.TAUREN, Genders.Female) => ChrModels.TAUREN_FEMALE,
                (Races.GNOME, Genders.Male) => ChrModels.GNOME_MALE,
                (Races.GNOME, Genders.Female) => ChrModels.GNOME_FEMALE,
                (Races.TROLL, Genders.Male) => ChrModels.TROLL_MALE,
                (Races.TROLL, Genders.Female) => ChrModels.TROLL_FEMALE,
                (Races.GOBLIN, Genders.Male) => ChrModels.GOBLIN_MALE,
                (Races.GOBLIN, Genders.Female) => ChrModels.GOBLIN_FEMALE,
                (Races.BLOOD_ELF, Genders.Male) => ChrModels.BLOOD_ELF_MALE,
                (Races.BLOOD_ELF, Genders.Female) => ChrModels.BLOOD_ELF_FEMALE,
                (Races.DRAENEI, Genders.Male) => ChrModels.DRAENEI_MALE,
                (Races.DRAENEI, Genders.Female) => ChrModels.DRAENEI_FEMALE,
                (Races.FEL_ORC, Genders.Male) => ChrModels.FEL_ORC_MALE,
                (Races.FEL_ORC, Genders.Female) => ChrModels.FEL_ORC_FEMALE,
                (Races.NAGA, Genders.Male) => ChrModels.NAGA_MALE,
                (Races.NAGA, Genders.Female) => ChrModels.NAGA_FEMALE,
                (Races.BROKEN, Genders.Male) => ChrModels.BROKEN_MALE,
                (Races.BROKEN, Genders.Female) => ChrModels.BROKEN_FEMALE,
                (Races.SKELETON, Genders.Male) => ChrModels.SKELETON_MALE,
                (Races.SKELETON, Genders.Female) => ChrModels.SKELETON_FEMALE,
                (Races.VRYKUL, Genders.Male) => ChrModels.VRYKUL_MALE,
                (Races.VRYKUL, Genders.Female) => ChrModels.VRYKUL_FEMALE,
                (Races.TUSKARR, Genders.Male) => ChrModels.TUSKARR_MALE,
                (Races.TUSKARR, Genders.Female) => ChrModels.TUSKARR_FEMALE,
                (Races.FOREST_TROLL, Genders.Male) => ChrModels.FOREST_TROLL_MALE,
                (Races.FOREST_TROLL, Genders.Female) => ChrModels.FOREST_TROLL_FEMALE,
                (Races.TAUNKA, Genders.Male) => ChrModels.TAUNKA_MALE,
                (Races.TAUNKA, Genders.Female) => ChrModels.TAUNKA_FEMALE,
                (Races.NORTHREND_SKELETON, Genders.Male) => ChrModels.NORTHREND_SKELETON_MALE,
                (Races.NORTHREND_SKELETON, Genders.Female) => ChrModels.NORTHREND_SKELETON_FEMALE,
                (Races.ICE_TROLL, Genders.Male) => ChrModels.ICE_TROLL_MALE,
                (Races.ICE_TROLL, Genders.Female) => ChrModels.ICE_TROLL_FEMALE,
                (Races.WORGEN, Genders.Male) => ChrModels.WORGEN_MALE,
                (Races.WORGEN, Genders.Female) => ChrModels.WORGEN_FEMALE,
                (Races.GILNEAN_HUMAN, Genders.Male) => ChrModels.GILNEAN_MALE,
                (Races.GILNEAN_HUMAN, Genders.Female) => ChrModels.GILNEAN_FEMALE,
                (Races.PANDAREN, Genders.Male) => ChrModels.PANDAREN_MALE,
                (Races.PANDAREN, Genders.Female) => ChrModels.PANDAREN_FEMALE,
                (Races.TUSHUI_PANDAREN, Genders.Male) => ChrModels.PANDAREN_MALE, // ??
                (Races.TUSHUI_PANDAREN, Genders.Female) => ChrModels.PANDAREN_FEMALE, // ??
                (Races.HUOJIN_PANDAREN, Genders.Male) => ChrModels.PANDAREN_MALE, // ??
                (Races.HUOJIN_PANDAREN, Genders.Female) => ChrModels.PANDAREN_FEMALE, // ??
                (Races.NIGHTBORNE, Genders.Male) => ChrModels.NIGHTBORN_MALE,
                (Races.NIGHTBORNE, Genders.Female) => ChrModels.NIGHTBORN_FEMALE,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.Male) => ChrModels.HIGHMOUNTAIN_TAUREN_MALE,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.Female) => ChrModels.HIGHMOUNTAIN_TAUREN_FEMALE,
                (Races.VOID_ELF, Genders.Male) => ChrModels.VOID_ELF_MALE,
                (Races.VOID_ELF, Genders.Female) => ChrModels.VOID_ELF_FEMALE,
                (Races.LIGHTFORGED_DRAENEI, Genders.Male) => ChrModels.LIGHTFORGED_DRAENEI_MALE,
                (Races.LIGHTFORGED_DRAENEI, Genders.Female) => ChrModels.LIGHTFORGED_DRAENEI_FEMALE,
                (Races.ZANDALARI_TROLL, Genders.Male) => ChrModels.ZANDALARI_MALE,
                (Races.ZANDALARI_TROLL, Genders.Female) => ChrModels.ZANDALARI_FEMALE,
                (Races.KUL_TIRAN, Genders.Male) => ChrModels.KUL_TIRAN_MALE,
                (Races.KUL_TIRAN, Genders.Female) => ChrModels.KUL_TIRAN_FEMALE,
                (Races.THIN_HUMAN, Genders.Male) => ChrModels.THIN_HUMAN_MALE,
                (Races.THIN_HUMAN, Genders.Female) => ChrModels.THIN_HUMAN_FEMALE,
                (Races.DARK_IRON_DWARF, Genders.Male) => ChrModels.DARK_IRON_DWARF_MALE,
                (Races.DARK_IRON_DWARF, Genders.Female) => ChrModels.DARK_IRON_DWARF_FEMALE,
                (Races.VULPERA, Genders.Male) => ChrModels.VOID_ELF_MALE,
                (Races.VULPERA, Genders.Female) => ChrModels.VOID_ELF_FEMALE,
                (Races.MAGHAR_ORC, Genders.Male) => ChrModels.MAGHAR_ORC_MALE,
                (Races.MAGHAR_ORC, Genders.Female) => ChrModels.MAGHAR_ORC_FEMALE,
                (Races.MECHAGNOME, Genders.Male) => ChrModels.MECHAGNOME_MALE,
                (Races.MECHAGNOME, Genders.Female) => ChrModels.MECHAGNOME_FEMALE,
                _ => null
            };
        }

        public bool IsDruidFormCustomization(ChrCustomizationOption option)
        {
            var druidForms = new List<int>() { 6, 7, 9, 10, 11, 12 }; // IDs gotten from ChrCustomizationCategory
            return druidForms.Any(d => d == option.ChrCustomizationCategoryId);
        }

        public int GetModelIdByRaceAndGenders(Races race, Genders gender, Dictionary<int, int?> customizations)
        {

            /* Comments:
             * These models are from CreatureModelData. The FileDataId points correctly to character/{race}/{gender}/{race}{gender}.m2.
             * Most have 1 but some have 0 (worgen) in SizeClass. Also, Flags are different. Idk what they are all used for.
             * */


            // For orcs and mag'har orcs male
            bool upright = true;
            if (gender == Genders.Male && race == Races.ORC)
            {
                upright = customizations.Any(c => c.Value == 439);
            }
            else if (gender == Genders.Male && race == Races.MAGHAR_ORC)
            {
                upright = customizations.Any(c => c.Value == 3427);
            }


            return (race, gender) switch
            {
                (Races.HUMAN, Genders.Male) => 7661,
                (Races.HUMAN, Genders.Female) => 7599,
                (Races.ORC, Genders.Male) => upright ? 10882 : 6838,
                (Races.ORC, Genders.Female) => 7200,
                (Races.DWARF, Genders.Male) => 5408,
                (Races.DWARF, Genders.Female) => 7203,
                (Races.NIGHT_ELF, Genders.Male) => 7369,
                (Races.NIGHT_ELF, Genders.Female) => 7300,
                (Races.UNDEAD, Genders.Male) => 7233,
                (Races.UNDEAD, Genders.Female) => 7578,
                (Races.TAUREN, Genders.Male) => 7399,
                (Races.TAUREN, Genders.Female) => 7576,
                (Races.GNOME, Genders.Male) => 6837,
                (Races.GNOME, Genders.Female) => 7130,
                (Races.TROLL, Genders.Male) => 7778,
                (Races.TROLL, Genders.Female) => 7793,
                (Races.GOBLIN, Genders.Male) => 831,
                (Races.GOBLIN, Genders.Female) => 832,
                (Races.BLOOD_ELF, Genders.Male) => 8102,
                (Races.BLOOD_ELF, Genders.Female) => 8103,
                (Races.DRAENEI, Genders.Male) => 7629,
                (Races.DRAENEI, Genders.Female) => 7692,
                (Races.WORGEN, Genders.Male) => 3141,
                (Races.WORGEN, Genders.Female) => 3142,
                (Races.DARK_IRON_DWARF, Genders.Male) => 10784,
                (Races.DARK_IRON_DWARF, Genders.Female) => 10785,
                (Races.MAGHAR_ORC, Genders.Male) => upright ? 10883 : 10844, // Referencing orc?
                (Races.MAGHAR_ORC, Genders.Female) => 10845, // Referencing orc?
                (Races.PANDAREN, Genders.Male) => 3967,
                (Races.PANDAREN, Genders.Female) => 3968,
                (Races.KUL_TIRAN, Genders.Male) => 10531,
                (Races.KUL_TIRAN, Genders.Female) => 10532,
                (Races.NIGHTBORNE, Genders.Male) => 9930,
                (Races.NIGHTBORNE, Genders.Female) => 9931,
                (Races.VOID_ELF, Genders.Male) => 9934,
                (Races.VOID_ELF, Genders.Female) => 9935,
                (Races.MECHAGNOME, Genders.Male) => 11488,
                (Races.MECHAGNOME, Genders.Female) => 11489,
                (Races.VULPERA, Genders.Male) => 10786,
                (Races.VULPERA, Genders.Female) => 10787,
                (Races.ZANDALARI_TROLL, Genders.Male) => 10394,
                (Races.ZANDALARI_TROLL, Genders.Female) => 10395,
                (Races.LIGHTFORGED_DRAENEI, Genders.Male) => 9936,
                (Races.LIGHTFORGED_DRAENEI, Genders.Female) => 9937,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.Male) => 9932,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.Female) => 9933,

                _ => throw new NotImplementedException($"Race and Gender combination {race} + {gender} is not implemented");
            };
        }
    }
}
