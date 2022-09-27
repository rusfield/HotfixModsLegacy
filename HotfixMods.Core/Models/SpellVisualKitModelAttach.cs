using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class SpellVisualKitModelAttach : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }


        // TODO: Check properties!!

        public decimal Scale { get; set; }
        public decimal ScaleVariation { get; set; }
        public int AttachmentId { get; set; }
        public int StartAnimId { get; set; }
        public int EndAnimId { get; set; }
        public int AnimKitId { get; set; }
        public int AnimId { get; set; }
        public int PositionerId { get; set; }
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public decimal Roll { get; set; }
        public decimal YawVariation { get; set; }
        public decimal PitchVariation { get; set; }
        public decimal RollVariation { get; set; }
        public decimal Offset0 { get; set; }
        public decimal Offset1 { get; set; }
        public decimal Offset2 { get; set; }
        public decimal OffsetVariation0 { get; set; }
        public decimal OffsetVariation1 { get; set; }
        public decimal OffsetVariation2 { get; set; }
        public int Flags { get; set; }
        public int LowDefModelAttachId { get; set; }
        public decimal StartDelay { get; set; }
        [Column("Field_901")]
        public int Field_9_0_1_33978_021 { get; set; }


        public int SpellVisualEffectNameId { get; set; }
        public int ParentSpellVisualKitId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
