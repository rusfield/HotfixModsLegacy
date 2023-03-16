using HotfixMods.Core.Models.TrinityCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.TrinityCore.Client
{
    public partial class TrinityCoreClient
    {
        string creatureData_path = Path.Combine("src", "server", "game", "Entities", "Creature", "CreatureData.h");
        string trainer_path = Path.Combine("src", "server", "game", "Entities", "Creature", "Trainer.h");
        string item_path = Path.Combine("src", "server", "game", "Entities", "Item", "Item.h");
        string sharedDefines_path = Path.Combine("src", "server", "game", "Miscellaneous", "SharedDefines.h");
        string movement_path = Path.Combine("src", "server", "game", "Movement", "MovementDefines.h");
        string unitDefines_path = Path.Combine("src", "server", "game", "Entities", "Unit", "UnitDefines.h");


        bool Equal(string input1, string input2)
        {
            return input1.Equals(input2, StringComparison.InvariantCultureIgnoreCase);
        }

        string UnderscoreToCase(string value)
        {
            string result = "";
            var words = value.ToString().Split("_");
            foreach (var word in words)
            {
                if (word.Length > 1)
                    result += $"{word.Substring(0, 1).ToUpper()}{word.Substring(1, word.Length - 1).ToLower()} ";
                else if (word.Length == 1)
                    result += $"{word.ToString().ToUpper()} ";
            }
            return result;
        }

        bool ValueIsValid(string input)
        {
            if (input.Contains("|"))
            {
                // Value is most likely an aggregated flag value that contains of other flag values.
                // Ex: SPELL_SCHOOL_MASK_SPELL = (SPELL_SCHOOL_MASK_FIRE | SPELL_SCHOOL_MASK_NATURE | SPELL_SCHOOL_MASK_FROST | SPELL_SCHOOL_MASK_SHADOW | SPELL_SCHOOL_MASK_ARCANE)
                return false;
            }
            else if(input.Contains("&"))
            {
                // Same as above.
                // Ex: (0xFFFFFFFF & ~UNIT_FLAG2_DISALLOWED) 
                return false;
            }
            return true;
        }

        string ConvertIfHex<T>(string input)
        {
            if (input.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                string type = typeof(T).ToString();
                input = type switch
                {
                    "System.SByte" => Convert.ToSByte(input, 16).ToString(),
                    "System.Int16" => Convert.ToInt16(input, 16).ToString(),
                    "System.Int32" => Convert.ToInt32(input, 16).ToString(),
                    "System.Int64" => Convert.ToInt64(input, 16).ToString(),
                    "System.Byte" => Convert.ToByte(input, 16).ToString(),
                    "System.UInt16" => Convert.ToUInt16(input, 16).ToString(),
                    "System.UInt32" => Convert.ToUInt32(input, 16).ToString(),
                    "System.UInt64" => Convert.ToUInt64(input, 16).ToString(),
                    _ => throw new Exception($"{input} does not have convertion for type {type}.")
                };
            }
            return input;
        }

        string ConvertIfShifting<T>(Dictionary<T, string> data, string input)
        {
            if (input.Contains("<<") && data.Any())
            {
                // Currently hardcoded for this format: "(1 << ENUM_NAME_OF_PREVIOUS_VALUE)"

                var previousValue = Convert.ToUInt64(data.Last().Key.ToString());
                if (previousValue == 0)
                    input = "1";
                else
                    input = (previousValue * 2).ToString();
            }
            return input;
        }
    }
}
