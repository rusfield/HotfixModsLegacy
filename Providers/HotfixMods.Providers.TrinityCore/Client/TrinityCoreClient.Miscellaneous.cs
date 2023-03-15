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
    }
}
