using HotfixMods.Core.Enums.Db2;
using HotfixMods.Core.Enums.TrinityCore;
using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;
using HotfixMods.Core.Models.TrinityCore;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Extensions;
using System.Collections;

namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        Dictionary<int, Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> customizationCache = new();

        public async Task<Dictionary<uint, string>> GetAvailableDisplayOptions(uint creatureId)
        {
            try
            {
                var result = new Dictionary<uint, string>();
                var creatureTemplateModels = await GetAsync<CreatureTemplateModel>(DefaultCallback, DefaultProgress, true, false, new DbParameter(nameof(CreatureTemplateModel.CreatureID), creatureId));
                foreach (var model in creatureTemplateModels)
                {
                    string name = model.Idx.ToString();
                    var creatureDisplayInfo = await GetSingleAsync<CreatureDisplayInfo>(new DbParameter(nameof(CreatureDisplayInfo.ID), model.CreatureDisplayID));
                    if (creatureDisplayInfo != null)
                    {
                        var creatureDisplayInfoExtra = await GetSingleAsync<CreatureDisplayInfoExtra>(new DbParameter(nameof(CreatureDisplayInfoExtra.ID), creatureDisplayInfo.ExtendedDisplayInfoID));
                        if (creatureDisplayInfoExtra != null && Enum.IsDefined(typeof(Gender), (int)creatureDisplayInfoExtra.DisplaySexID) && Enum.IsDefined(typeof(ChrRaceId), (int)creatureDisplayInfoExtra.DisplayRaceID))
                        {
                            var gender = (Gender)(int)creatureDisplayInfoExtra.DisplaySexID;
                            var race = (ChrRaceId)(int)creatureDisplayInfoExtra.DisplayRaceID;
                            name += $" - {race.ToDisplayString()} {gender.ToDisplayString()}";
                        }
                    }
                    else
                    {
                        name += $" - {model.CreatureDisplayID}";
                    }
                    result.Add(model.Idx, name);
                }
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public async Task<Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> GetCustomizationOptions(int chrRaceId, int gender, bool includeDruidForms = false)
        {
            try
            {
                var result = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();
                var chrRaceXChrModel = await GetSingleAsync<ChrRaceXChrModel>(new DbParameter(nameof(ChrRaceXChrModel.ChrRacesID), chrRaceId), new DbParameter(nameof(ChrRaceXChrModel.Sex), gender));
                if (null == chrRaceXChrModel)
                {
                    // No customizations for this combination, or possibly old/missing data from ChrRaceXChrModel.
                    return result;
                }
                else if (customizationCache.ContainsKey(chrRaceXChrModel.ChrModelID))
                {
                    result = customizationCache[chrRaceXChrModel.ChrModelID];
                }
                else
                {
                    var options = await GetAsync<ChrCustomizationOption>(DefaultCallback, DefaultProgress, false, true, new DbParameter(nameof(ChrCustomizationOption.ChrModelID), chrRaceXChrModel.ChrModelID));
                    foreach (var option in options)
                    {
                        var choices = await GetAsync<ChrCustomizationChoice>(DefaultCallback, DefaultProgress, false, true, new DbParameter(nameof(ChrCustomizationChoice.ChrCustomizationOptionID), option.ID));
                        result.Add(option, choices);
                    }

                    // In case it has been added elsewhere in the meantime
                    if (!customizationCache.ContainsKey(chrRaceXChrModel.ChrModelID) && result.Count > 0)
                        customizationCache.Add(chrRaceXChrModel.ChrModelID, result);
                }

                if (!includeDruidForms)
                {
                    var filteredResult = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();
                    foreach (var option in result)
                    {
                        // Currently, only druid forms are named like "Moonkin Form", etc.
                        // Edit this condition if it should affect new customizations at some point.
                        if (!option.Key.Name.ToLower().EndsWith(" form"))
                            filteredResult.Add(option.Key, option.Value);
                    }
                    return filteredResult;
                }
                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return new();
        }

        public Dictionary<ushort, string> GetModelIds()
        {
            var result = new Dictionary<ushort, string>();
            result.Add(7661, "Human Male");
            result.Add(7599, "Human Female");
            result.Add(6838, "Orc Male");
            result.Add(10882, "Orc Male (Upright)");
            result.Add(7200, "Orc Female");
            result.Add(5408, "Dwarf Male");
            result.Add(7203, "Dwarf Female");
            result.Add(7369, "Night Elf Male");
            result.Add(7300, "Night Elf Female");
            result.Add(7233, "Scourge Male");
            result.Add(7578, "Scourge Female");
            result.Add(7399, "Tauren Male");
            result.Add(7576, "Tauren Female");
            result.Add(6837, "Gnome Male");
            result.Add(7130, "Gnome Female");
            result.Add(7778, "Troll Male");
            result.Add(7793, "Troll Female");
            result.Add(831, "Goblin Male");
            result.Add(832, "Goblin Female");
            result.Add(8102, "Blood Elf Male");
            result.Add(8103, "Blood Elf Female");
            result.Add(7629, "Draenei Male");
            result.Add(7692, "Draenei Female");
            result.Add(3141, "Worgen Male");
            result.Add(3142, "Worgen Female");
            result.Add(10784, "Dark Iron Dwarf Male");
            result.Add(10785, "Dark Iron Dwarf Female");
            result.Add(10844, "Maghar Orc Male");
            result.Add(10883, "Maghar Orc Male (Upright)");
            result.Add(10845, "Maghar Orc Female");
            result.Add(3967, "Pandaren Male");
            result.Add(3968, "Pandaren Female");
            result.Add(10531, "Kul Tiran Male");
            result.Add(10532, "Kul Tiran Female");
            result.Add(9930, "Nightborne Male");
            result.Add(9931, "Nightborne Female");
            result.Add(9934, "Void Elf Male");
            result.Add(9935, "Void Elf Female");
            result.Add(11488, "Mechagnome Male");
            result.Add(11489, "Mechagnome Female");
            result.Add(10786, "Vulpera Male");
            result.Add(10787, "Vulpera Female");
            result.Add(10394, "Zandalari Troll Male");
            result.Add(10395, "Zandalari Troll Female");
            result.Add(9936, "Lightforged Draenei Male");
            result.Add(9937, "Lightforged Draenei Female");
            result.Add(9932, "Highmountain Tauren Male");
            result.Add(9933, "Highmountain Tauren Female");

            return result;
        }

        sbyte CharacterInventorySlotToNpcModelItemSlot(byte inventorySlot)
        {
            if (Enum.IsDefined(typeof(CharacterInventorySlot), (int)inventorySlot))
            {
                return (CharacterInventorySlot)inventorySlot switch
                {
                    CharacterInventorySlot.FEET => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.FEET,
                    CharacterInventorySlot.LEGS => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.LEGS,
                    CharacterInventorySlot.WAIST => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.WAIST,
                    CharacterInventorySlot.CHEST => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.CHEST,
                    CharacterInventorySlot.SHIRT => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.SHIRT,
                    CharacterInventorySlot.TABARD => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.TABARD,
                    CharacterInventorySlot.SHOULDERS => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.SHOULDERS,
                    CharacterInventorySlot.WRISTS => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.WRISTS,
                    CharacterInventorySlot.HANDS => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.HANDS,
                    CharacterInventorySlot.HEAD => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.HEAD,
                    CharacterInventorySlot.BACK => (sbyte)NpcModelItemSlotDisplayInfoItemSlot.CAPE,
                    _ => 0
                };
            }
            return 0;
        }

        ushort GetModelIdByRaceAndGenders(sbyte race, sbyte gender, List<CreatureDisplayInfoOption> creatureDisplayInfoOption)
        {
            // These models are from CreatureModelData. The FileDataId points correctly to character/{race}/{gender}/{race}{gender}.m2.

            // For orcs and mag'har orcs male
            bool upright = true;
            if (gender == (int)Gender.MALE && race == (int)ChrRaceId.ORC)
            {
                upright = creatureDisplayInfoOption.Any(c => c.ChrCustomizationChoiceID == 439);
            }
            else if (gender == (int)Gender.MALE && race == (int)ChrRaceId.MAGHAR_ORC)
            {
                upright = creatureDisplayInfoOption.Any(c => c.ChrCustomizationChoiceID == 3427);
            }


            return ((ChrRaceId)race, (Gender)gender) switch
            {
                (ChrRaceId.HUMAN, Gender.MALE) => 7661,
                (ChrRaceId.HUMAN, Gender.FEMALE) => 7599,
                (ChrRaceId.ORC, Gender.MALE) => (ushort)(upright ? 10882 : 6838),
                (ChrRaceId.ORC, Gender.FEMALE) => 7200,
                (ChrRaceId.DWARF, Gender.MALE) => 5408,
                (ChrRaceId.DWARF, Gender.FEMALE) => 7203,
                (ChrRaceId.NIGHT_ELF, Gender.MALE) => 7369,
                (ChrRaceId.NIGHT_ELF, Gender.FEMALE) => 7300,
                (ChrRaceId.SCOURGE, Gender.MALE) => 7233,
                (ChrRaceId.SCOURGE, Gender.FEMALE) => 7578,
                (ChrRaceId.TAUREN, Gender.MALE) => 7399,
                (ChrRaceId.TAUREN, Gender.FEMALE) => 7576,
                (ChrRaceId.GNOME, Gender.MALE) => 6837,
                (ChrRaceId.GNOME, Gender.FEMALE) => 7130,
                (ChrRaceId.TROLL, Gender.MALE) => 7778,
                (ChrRaceId.TROLL, Gender.FEMALE) => 7793,
                (ChrRaceId.GOBLIN, Gender.MALE) => 831,
                (ChrRaceId.GOBLIN, Gender.FEMALE) => 832,
                (ChrRaceId.BLOOD_ELF, Gender.MALE) => 8102,
                (ChrRaceId.BLOOD_ELF, Gender.FEMALE) => 8103,
                (ChrRaceId.DRAENEI, Gender.MALE) => 7629,
                (ChrRaceId.DRAENEI, Gender.FEMALE) => 7692,
                (ChrRaceId.WORGEN, Gender.MALE) => 3141,
                (ChrRaceId.WORGEN, Gender.FEMALE) => 3142,
                (ChrRaceId.DARK_IRON_DWARF, Gender.MALE) => 10784,
                (ChrRaceId.DARK_IRON_DWARF, Gender.FEMALE) => 10785,
                (ChrRaceId.MAGHAR_ORC, Gender.MALE) => (ushort)(upright ? 10883 : 10844),
                (ChrRaceId.MAGHAR_ORC, Gender.FEMALE) => 10845,
                (ChrRaceId.PANDAREN_ALLIANCE, Gender.MALE) => 3967,
                (ChrRaceId.PANDAREN_ALLIANCE, Gender.FEMALE) => 3968,
                (ChrRaceId.PANDAREN_HORDE, Gender.MALE) => 3967,
                (ChrRaceId.PANDAREN_NEUTRAL, Gender.MALE) => 3967,
                (ChrRaceId.PANDAREN_NEUTRAL, Gender.FEMALE) => 3968,
                (ChrRaceId.KUL_TIRAN, Gender.MALE) => 10531,
                (ChrRaceId.KUL_TIRAN, Gender.FEMALE) => 10532,
                (ChrRaceId.NIGHTBORNE, Gender.MALE) => 9930,
                (ChrRaceId.NIGHTBORNE, Gender.FEMALE) => 9931,
                (ChrRaceId.VOID_ELF, Gender.MALE) => 9934,
                (ChrRaceId.VOID_ELF, Gender.FEMALE) => 9935,
                (ChrRaceId.MECHAGNOME, Gender.MALE) => 11488,
                (ChrRaceId.MECHAGNOME, Gender.FEMALE) => 11489,
                (ChrRaceId.VULPERA, Gender.MALE) => 10786,
                (ChrRaceId.VULPERA, Gender.FEMALE) => 10787,
                (ChrRaceId.ZANDALARI_TROLL, Gender.MALE) => 10394,
                (ChrRaceId.ZANDALARI_TROLL, Gender.FEMALE) => 10395,
                (ChrRaceId.LIGHTFORGED_DRAENEI, Gender.MALE) => 9936,
                (ChrRaceId.LIGHTFORGED_DRAENEI, Gender.FEMALE) => 9937,
                (ChrRaceId.HIGHMOUNTAIN_TAUREN, Gender.MALE) => 9932,
                (ChrRaceId.HIGHMOUNTAIN_TAUREN, Gender.FEMALE) => 9933,
                _ => 0
            };
        }

        async Task SetIdAndVerifiedBuild(CreatureDto dto)
        {
            // Step 1: Init IDs of single entities
            var hotfixModsEntityId = await GetIdByConditionsAsync<HotfixModsEntity>(dto.HotfixModsEntity.ID, dto.IsUpdate);
            var creatureTemplateId = await GetIdByConditionsAsync<CreatureTemplate>((int)dto.CreatureTemplate.Entry, dto.IsUpdate);
            var creatureDisplayInfoId = await GetIdByConditionsAsync<CreatureDisplayInfo>(dto.CreatureDisplayInfo.ID, dto.IsUpdate);
            var creatureDisplayInfoExtraId = await GetIdByConditionsAsync<CreatureDisplayInfoExtra>(dto.CreatureDisplayInfoExtra?.ID, dto.IsUpdate);

            // Step 2: Prepare IDs of list entities
            var nextNpcModelItemSlotDisplayInfo = await GetNextIdAsync<NpcModelItemSlotDisplayInfo>();
            var nextCreatureDisplayInfoOption = await GetNextIdAsync<CreatureDisplayInfoOption>();

            // Step 3: Populate entities
            dto.HotfixModsEntity.ID = hotfixModsEntityId;
            dto.HotfixModsEntity.RecordID = creatureTemplateId;
            dto.HotfixModsEntity.VerifiedBuild = VerifiedBuild;

            dto.CreatureTemplate.Entry = (uint)creatureTemplateId;
            dto.CreatureTemplate.VerifiedBuild = VerifiedBuild;

            dto.CreatureTemplateModel.CreatureID = (uint)creatureTemplateId;
            dto.CreatureTemplateModel.CreatureDisplayID = (uint)creatureDisplayInfoId;
            dto.CreatureTemplateModel.VerifiedBuild = VerifiedBuild;

            dto.CreatureDisplayInfo.ID = creatureDisplayInfoId;
            dto.CreatureDisplayInfo.ExtendedDisplayInfoID = (int)creatureDisplayInfoExtraId;
            dto.CreatureDisplayInfo.VerifiedBuild = VerifiedBuild;


            dto.CreatureModelInfo.DisplayID = (uint)creatureDisplayInfoId;
            dto.CreatureModelInfo.VerifiedBuild = VerifiedBuild;

            if (dto.CreatureEquipTemplate != null)
            {
                dto.CreatureEquipTemplate.CreatureID = (uint)creatureTemplateId;
                dto.CreatureEquipTemplate.VerifiedBuild = VerifiedBuild;
            }

            if (dto.CreatureTemplateAddon != null)
            {
                dto.CreatureTemplateAddon.Entry = (uint)creatureTemplateId;
                //dto.CreatureTempalteAddon.VerifiedBuild = VerifiedBuild; // property does not currently exist
            }

            if (dto.CreatureDisplayInfoExtra != null)
            {
                dto.CreatureDisplayInfoExtra.ID = creatureDisplayInfoExtraId;
                dto.CreatureDisplayInfoExtra.VerifiedBuild = VerifiedBuild;

                if (dto.NpcModelItemSlotDisplayInfo?.Any() ?? false)
                {
                    dto.NpcModelItemSlotDisplayInfo.ForEach(item =>
                    {
                        item.NpcModelID = (int)creatureDisplayInfoExtraId;
                        item.ID = nextNpcModelItemSlotDisplayInfo++;
                        item.VerifiedBuild = VerifiedBuild;
                    });
                }

                if (dto.CreatureDisplayInfoOption?.Any() ?? false)
                {
                    dto.CreatureDisplayInfoOption.ForEach(item =>
                    {
                        item.CreatureDisplayInfoExtraID = (int)creatureDisplayInfoExtraId;
                        item.ID = nextCreatureDisplayInfoOption++;
                        item.VerifiedBuild = VerifiedBuild;
                    });
                }
            }
        }
        bool IsWeaponSlot(int slot)
        {
            return slot == 15 || slot == 16 || slot == 17; //MAIN_HAND, OFF_HAND and RANGED
        }
    }
}
