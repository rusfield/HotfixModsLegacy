using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class CreatureDisplayInfo : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int SoundId { get; set; }
        public int ExtendedDisplayInfoId { get; set; }
        public Genders Gender { get; set; }
        public int SizeClass { get; set; }
        public int CreatureModelAlpha { get; set; }
        public decimal CreatureModelScale { get; set; }
        public decimal PetInstanceScale { get; set; }
        public int UnarmedWeaponType { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
