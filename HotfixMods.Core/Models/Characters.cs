using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HotfixMods.Core.Models
{
    public class Characters : ICharactersSchema
    {
        [Key]
        public int Guid { get; set; }
        public string Name { get; set; }
        public Races Race { get; set; }
        public Genders Gender { get; set; }
        public int Level { get; set; }
    }
}
