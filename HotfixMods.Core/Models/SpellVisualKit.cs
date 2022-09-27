using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualKit : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        // TODO: Investigate properties
        [Column("Flags1")]
        public int Flags0 { get; set; }
        [Column("Flags2")]
        public int Flags1 { get; set; }
        [Column("FallbackPriority")]
        public int ClutterLevel { get; set; }
        public int FallbackSpellVisualKitId { get; set; }
        public int DelayMin { get; set; }
        public int DelayMax { get; set; }   
        public int VerifiedBuild { get; set; }
    }
}
