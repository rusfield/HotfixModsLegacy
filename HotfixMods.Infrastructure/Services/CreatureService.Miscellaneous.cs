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

            var options = await _db2.GetAsync<ChrCustomizationOption>(c => c.ChrModelId == chrModel);
            foreach (var option in options)
            {
                if (!includeDruidForms && IsDruidFormCustomization(option))
                    continue;

                var choices = (await _db2.GetAsync<ChrCustomizationChoice>(c => c.ChrCustomizationOptionId == option.Id)).ToList();
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

        protected ChrModels? ConvertRaceAndGenderToChrModel(Races race, Genders gender)
        {
            return (race, gender) switch
            {
                (Races.HUMAN, Genders.MALE) => ChrModels.HUMAN_MALE,
                (Races.HUMAN, Genders.FEMALE) => ChrModels.HUMAN_FEMALE,
                (Races.ORC, Genders.MALE) => ChrModels.ORC_MALE,
                (Races.ORC, Genders.FEMALE) => ChrModels.ORC_FEMALE,
                (Races.DWARF, Genders.MALE) => ChrModels.DWARF_MALE,
                (Races.DWARF, Genders.FEMALE) => ChrModels.DWARF_FEMALE,
                (Races.NIGHT_ELF, Genders.MALE) => ChrModels.NIGHT_ELF_MALE,
                (Races.NIGHT_ELF, Genders.FEMALE) => ChrModels.NIGHT_ELF_FEMALE,
                (Races.UNDEAD, Genders.MALE) => ChrModels.UNDEAD_MALE,
                (Races.UNDEAD, Genders.FEMALE) => ChrModels.UNDEAD_FEMALE,
                (Races.TAUREN, Genders.MALE) => ChrModels.TAUREN_MALE,
                (Races.TAUREN, Genders.FEMALE) => ChrModels.TAUREN_FEMALE,
                (Races.GNOME, Genders.MALE) => ChrModels.GNOME_MALE,
                (Races.GNOME, Genders.FEMALE) => ChrModels.GNOME_FEMALE,
                (Races.TROLL, Genders.MALE) => ChrModels.TROLL_MALE,
                (Races.TROLL, Genders.FEMALE) => ChrModels.TROLL_FEMALE,
                (Races.GOBLIN, Genders.MALE) => ChrModels.GOBLIN_MALE,
                (Races.GOBLIN, Genders.FEMALE) => ChrModels.GOBLIN_FEMALE,
                (Races.BLOOD_ELF, Genders.MALE) => ChrModels.BLOOD_ELF_MALE,
                (Races.BLOOD_ELF, Genders.FEMALE) => ChrModels.BLOOD_ELF_FEMALE,
                (Races.DRAENEI, Genders.MALE) => ChrModels.DRAENEI_MALE,
                (Races.DRAENEI, Genders.FEMALE) => ChrModels.DRAENEI_FEMALE,
                (Races.FEL_ORC, Genders.MALE) => ChrModels.FEL_ORC_MALE,
                (Races.FEL_ORC, Genders.FEMALE) => ChrModels.FEL_ORC_FEMALE,
                (Races.NAGA, Genders.MALE) => ChrModels.NAGA_MALE,
                (Races.NAGA, Genders.FEMALE) => ChrModels.NAGA_FEMALE,
                (Races.BROKEN, Genders.MALE) => ChrModels.BROKEN_MALE,
                (Races.BROKEN, Genders.FEMALE) => ChrModels.BROKEN_FEMALE,
                (Races.SKELETON, Genders.MALE) => ChrModels.SKELETON_MALE,
                (Races.SKELETON, Genders.FEMALE) => ChrModels.SKELETON_FEMALE,
                (Races.VRYKUL, Genders.MALE) => ChrModels.VRYKUL_MALE,
                (Races.VRYKUL, Genders.FEMALE) => ChrModels.VRYKUL_FEMALE,
                (Races.TUSKARR, Genders.MALE) => ChrModels.TUSKARR_MALE,
                (Races.TUSKARR, Genders.FEMALE) => ChrModels.TUSKARR_FEMALE,
                (Races.FOREST_TROLL, Genders.MALE) => ChrModels.FOREST_TROLL_MALE,
                (Races.FOREST_TROLL, Genders.FEMALE) => ChrModels.FOREST_TROLL_FEMALE,
                (Races.TAUNKA, Genders.MALE) => ChrModels.TAUNKA_MALE,
                (Races.TAUNKA, Genders.FEMALE) => ChrModels.TAUNKA_FEMALE,
                (Races.NORTHREND_SKELETON, Genders.MALE) => ChrModels.NORTHREND_SKELETON_MALE,
                (Races.NORTHREND_SKELETON, Genders.FEMALE) => ChrModels.NORTHREND_SKELETON_FEMALE,
                (Races.ICE_TROLL, Genders.MALE) => ChrModels.ICE_TROLL_MALE,
                (Races.ICE_TROLL, Genders.FEMALE) => ChrModels.ICE_TROLL_FEMALE,
                (Races.WORGEN, Genders.MALE) => ChrModels.WORGEN_MALE,
                (Races.WORGEN, Genders.FEMALE) => ChrModels.WORGEN_FEMALE,
                (Races.GILNEAN_HUMAN, Genders.MALE) => ChrModels.GILNEAN_MALE,
                (Races.GILNEAN_HUMAN, Genders.FEMALE) => ChrModels.GILNEAN_FEMALE,
                (Races.PANDAREN, Genders.MALE) => ChrModels.PANDAREN_MALE,
                (Races.PANDAREN, Genders.FEMALE) => ChrModels.PANDAREN_FEMALE,
                (Races.TUSHUI_PANDAREN, Genders.MALE) => ChrModels.PANDAREN_MALE, // ??
                (Races.TUSHUI_PANDAREN, Genders.FEMALE) => ChrModels.PANDAREN_FEMALE, // ??
                (Races.HUOJIN_PANDAREN, Genders.MALE) => ChrModels.PANDAREN_MALE, // ??
                (Races.HUOJIN_PANDAREN, Genders.FEMALE) => ChrModels.PANDAREN_FEMALE, // ??
                (Races.NIGHTBORNE, Genders.MALE) => ChrModels.NIGHTBORN_MALE,
                (Races.NIGHTBORNE, Genders.FEMALE) => ChrModels.NIGHTBORN_FEMALE,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.MALE) => ChrModels.HIGHMOUNTAIN_TAUREN_MALE,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.FEMALE) => ChrModels.HIGHMOUNTAIN_TAUREN_FEMALE,
                (Races.VOID_ELF, Genders.MALE) => ChrModels.VOID_ELF_MALE,
                (Races.VOID_ELF, Genders.FEMALE) => ChrModels.VOID_ELF_FEMALE,
                (Races.LIGHTFORGED_DRAENEI, Genders.MALE) => ChrModels.LIGHTFORGED_DRAENEI_MALE,
                (Races.LIGHTFORGED_DRAENEI, Genders.FEMALE) => ChrModels.LIGHTFORGED_DRAENEI_FEMALE,
                (Races.ZANDALARI_TROLL, Genders.MALE) => ChrModels.ZANDALARI_MALE,
                (Races.ZANDALARI_TROLL, Genders.FEMALE) => ChrModels.ZANDALARI_FEMALE,
                (Races.KUL_TIRAN, Genders.MALE) => ChrModels.KUL_TIRAN_MALE,
                (Races.KUL_TIRAN, Genders.FEMALE) => ChrModels.KUL_TIRAN_FEMALE,
                (Races.THIN_HUMAN, Genders.MALE) => ChrModels.THIN_HUMAN_MALE,
                (Races.THIN_HUMAN, Genders.FEMALE) => ChrModels.THIN_HUMAN_FEMALE,
                (Races.DARK_IRON_DWARF, Genders.MALE) => ChrModels.DARK_IRON_DWARF_MALE,
                (Races.DARK_IRON_DWARF, Genders.FEMALE) => ChrModels.DARK_IRON_DWARF_FEMALE,
                (Races.VULPERA, Genders.MALE) => ChrModels.VOID_ELF_MALE,
                (Races.VULPERA, Genders.FEMALE) => ChrModels.VOID_ELF_FEMALE,
                (Races.MAGHAR_ORC, Genders.MALE) => ChrModels.MAGHAR_ORC_MALE,
                (Races.MAGHAR_ORC, Genders.FEMALE) => ChrModels.MAGHAR_ORC_FEMALE,
                (Races.MECHAGNOME, Genders.MALE) => ChrModels.MECHAGNOME_MALE,
                (Races.MECHAGNOME, Genders.FEMALE) => ChrModels.MECHAGNOME_FEMALE,
                _ => null
            };
        }

        protected bool IsDruidFormCustomization(ChrCustomizationOption option)
        {
            var druidForms = new List<int>() { 6, 7, 9, 10, 11, 12 }; // IDs gotten from ChrCustomizationCategory
            return druidForms.Any(d => d == option.ChrCustomizationCategoryId);
        }

        protected int GetModelIdByRaceAndGenders(Races race, Genders gender, Dictionary<int, int?> customizations)
        {

            /* Comments:
             * These models are from CreatureModelData. The FileDataId points correctly to character/{race}/{gender}/{race}{gender}.m2.
             * Most have 1 but some have 0 (worgen) in SizeClass. Also, Flags are different. Idk what they are all used for.
             * */


            // For orcs and mag'har orcs male
            bool upright = true;
            if (gender == Genders.MALE && race == Races.ORC)
            {
                upright = customizations.Any(c => c.Value == 439);
            }
            else if (gender == Genders.MALE && race == Races.MAGHAR_ORC)
            {
                upright = customizations.Any(c => c.Value == 3427);
            }


            return (race, gender) switch
            {
                (Races.HUMAN, Genders.MALE) => 7661,
                (Races.HUMAN, Genders.FEMALE) => 7599,
                (Races.ORC, Genders.MALE) => upright ? 10882 : 6838,
                (Races.ORC, Genders.FEMALE) => 7200,
                (Races.DWARF, Genders.MALE) => 5408,
                (Races.DWARF, Genders.FEMALE) => 7203,
                (Races.NIGHT_ELF, Genders.MALE) => 7369,
                (Races.NIGHT_ELF, Genders.FEMALE) => 7300,
                (Races.UNDEAD, Genders.MALE) => 7233,
                (Races.UNDEAD, Genders.FEMALE) => 7578,
                (Races.TAUREN, Genders.MALE) => 7399,
                (Races.TAUREN, Genders.FEMALE) => 7576,
                (Races.GNOME, Genders.MALE) => 6837,
                (Races.GNOME, Genders.FEMALE) => 7130,
                (Races.TROLL, Genders.MALE) => 7778,
                (Races.TROLL, Genders.FEMALE) => 7793,
                (Races.GOBLIN, Genders.MALE) => 831,
                (Races.GOBLIN, Genders.FEMALE) => 832,
                (Races.BLOOD_ELF, Genders.MALE) => 8102,
                (Races.BLOOD_ELF, Genders.FEMALE) => 8103,
                (Races.DRAENEI, Genders.MALE) => 7629,
                (Races.DRAENEI, Genders.FEMALE) => 7692,
                (Races.WORGEN, Genders.MALE) => 3141,
                (Races.WORGEN, Genders.FEMALE) => 3142,
                (Races.DARK_IRON_DWARF, Genders.MALE) => 10784,
                (Races.DARK_IRON_DWARF, Genders.FEMALE) => 10785,
                (Races.MAGHAR_ORC, Genders.MALE) => upright ? 10883 : 10844, // Referencing orc?
                (Races.MAGHAR_ORC, Genders.FEMALE) => 10845, // Referencing orc?
                (Races.PANDAREN, Genders.MALE) => 3967,
                (Races.PANDAREN, Genders.FEMALE) => 3968,
                (Races.KUL_TIRAN, Genders.MALE) => 10531,
                (Races.KUL_TIRAN, Genders.FEMALE) => 10532,
                (Races.NIGHTBORNE, Genders.MALE) => 9930,
                (Races.NIGHTBORNE, Genders.FEMALE) => 9931,
                (Races.VOID_ELF, Genders.MALE) => 9934,
                (Races.VOID_ELF, Genders.FEMALE) => 9935,
                (Races.MECHAGNOME, Genders.MALE) => 11488,
                (Races.MECHAGNOME, Genders.FEMALE) => 11489,
                (Races.VULPERA, Genders.MALE) => 10786,
                (Races.VULPERA, Genders.FEMALE) => 10787,
                (Races.ZANDALARI_TROLL, Genders.MALE) => 10394,
                (Races.ZANDALARI_TROLL, Genders.FEMALE) => 10395,
                (Races.LIGHTFORGED_DRAENEI, Genders.MALE) => 9936,
                (Races.LIGHTFORGED_DRAENEI, Genders.FEMALE) => 9937,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.MALE) => 9932,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.FEMALE) => 9933,

                _ => throw new NotImplementedException($"Race and Gender combination {race} + {gender} is not implemented")
            };
        }

        protected int GetDefaultSoundId(Races race, Genders gender)
        {
            // TODO: Needs more digging. 
            // Some missing, and some of them are missing crit attack sound.
            return (race, gender) switch
            {
                (Races.HUMAN, Genders.MALE) => 6847,
                (Races.HUMAN, Genders.FEMALE) => 6688,
                (Races.ORC, Genders.MALE) => 51,
                (Races.ORC, Genders.FEMALE) => 4314,
                (Races.DWARF, Genders.MALE) => 53,
                (Races.DWARF, Genders.FEMALE) => 54,
                (Races.NIGHT_ELF, Genders.MALE) => 55,
                (Races.NIGHT_ELF, Genders.FEMALE) => 56,
                (Races.UNDEAD, Genders.MALE) => 57,
                (Races.UNDEAD, Genders.FEMALE) => 58,
                (Races.TAUREN, Genders.MALE) => 59,
                (Races.TAUREN, Genders.FEMALE) => 60,
                (Races.GNOME, Genders.MALE) => 294,
                (Races.GNOME, Genders.FEMALE) => 295,
                (Races.TROLL, Genders.MALE) => 296,
                (Races.TROLL, Genders.FEMALE) => 297,
                (Races.GOBLIN, Genders.MALE) => 4315,
                (Races.GOBLIN, Genders.FEMALE) => 4316,
                (Races.BLOOD_ELF, Genders.MALE) => 2155,
                (Races.BLOOD_ELF, Genders.FEMALE) => 2156,
                (Races.DRAENEI, Genders.MALE) => 2153,
                (Races.DRAENEI, Genders.FEMALE) => 2154,
                (Races.WORGEN, Genders.MALE) => 3058,
                (Races.WORGEN, Genders.FEMALE) => 3061,
                (Races.DARK_IRON_DWARF, Genders.MALE) => 5958,
                (Races.DARK_IRON_DWARF, Genders.FEMALE) => 5959,
                (Races.MAGHAR_ORC, Genders.MALE) => 6087,
                (Races.MAGHAR_ORC, Genders.FEMALE) => 6086,
                (Races.PANDAREN, Genders.MALE) => 4560,
                (Races.PANDAREN, Genders.FEMALE) => 4559,
                (Races.KUL_TIRAN, Genders.MALE) => 6008,
                (Races.KUL_TIRAN, Genders.FEMALE) => 6296,
                (Races.NIGHTBORNE, Genders.MALE) => 5919,
                (Races.NIGHTBORNE, Genders.FEMALE) => 5918,
                (Races.VOID_ELF, Genders.MALE) => 5912,
                (Races.VOID_ELF, Genders.FEMALE) => 5913,
                (Races.MECHAGNOME, Genders.MALE) => 6644,
                (Races.MECHAGNOME, Genders.FEMALE) => 6643,
                (Races.VULPERA, Genders.MALE) => 6278,
                (Races.VULPERA, Genders.FEMALE) => 6279,
                (Races.ZANDALARI_TROLL, Genders.MALE) => 6453,
                (Races.ZANDALARI_TROLL, Genders.FEMALE) => 6448,
                (Races.LIGHTFORGED_DRAENEI, Genders.MALE) => 5917,
                (Races.LIGHTFORGED_DRAENEI, Genders.FEMALE) => 5916,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.MALE) => 5915,
                (Races.HIGHMOUNTAIN_TAUREN, Genders.FEMALE) => 5911,

                _ => 0
            };
        }

        protected bool IsWeaponSlot(CharacterInventorySlots slot)
        {
            return slot == CharacterInventorySlots.MAIN_HAND || slot == CharacterInventorySlots.OFF_HAND || slot == CharacterInventorySlots.RANGED;
        }
    }
}
