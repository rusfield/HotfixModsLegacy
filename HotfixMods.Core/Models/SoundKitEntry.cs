using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SoundKitEntry : IHotfixesSchema, IDb2
    {
        public int Id { get; set; }
        public int SoundKitId { get; set; }
        public int FileDataId { get; set; }
        public int Frequency { get; set; }
        public double Volume { get; set; }
    }
}
