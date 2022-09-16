using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class CreatureSoundData : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int SoundExertionId { get; set; }
        public int SoundExertionCriticalId { get; set; }
        public int SoundInjuryId { get; set; }
        public int SoundInjuryCriticalId { get; set; }
        public int SoundDeathId { get; set; }
    }
}
