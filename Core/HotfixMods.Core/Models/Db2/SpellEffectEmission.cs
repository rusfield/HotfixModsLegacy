﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class SpellEffectEmission
    {
        [IndexField]
        public int ID { get; set; } = 0;
        public decimal EmissionRate { get; set; } = 1;
        public decimal ModelScale { get; set; } = 1;
        public short AreaModelID { get; set; } = 0;
        public sbyte Flags { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }
}
