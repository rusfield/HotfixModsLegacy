using HotfixMods.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class GossipNpcOption
    {
        [IndexField]
        public int ID { get; set; } = 0;
        [Column("GossipNpcOption")]
        public int GossipNpcOptionValue { get; set; } = 0;
        public int LFGDungeonsID { get; set; } = 0;
        public int TrainerID { get; set; } = 0;
        public sbyte GarrFollowerTypeID { get; set; } = 0;
        public int CharShipmentID { get; set; } = 0;
        public int GarrTalentTreeID { get; set; } = 0;
        public int UiMapID { get; set; } = 0;
        public int UiItemInteractionID { get; set; } = 0;
        public int Unknown_1000_8 { get; set; } = 0;
        public int Unknown_1000_9 { get; set; } = 0;
        public int CovenantID { get; set; } = 0;
        public int GossipOptionID { get; set; } = 0;
        public int TraitTreeID { get; set; } = 0;
        public int ProfessionID { get; set; } = 0;
        public int Unknown_1002_14 { get; set; } = 0;
        public int NeighborhoodMapID { get; set; } = 0;
        public int SkillLineID { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
