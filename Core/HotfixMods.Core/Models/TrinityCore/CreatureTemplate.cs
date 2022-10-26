using System.ComponentModel.DataAnnotations.Schema;

namespace HotfixMods.Core.Models.TrinityCore
{
    public class CreatureTemplate 
    {
        public int Entry { get; set; }
        public string? Name { get; set; }
        public string? SubName { get; set; }
        public int MinLevel { get; set; }

        public decimal RangeVariance { get; set; }
        public decimal HoverHeight { get; set; }
        public bool RegenHealth { get; set; }

        public decimal HealthModifier { get; set; }
        public decimal HealthModifierExtra { get; set; }


        public int VerifiedBuild { get; set; }



    }
}
