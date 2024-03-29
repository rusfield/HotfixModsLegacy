﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureTemplateModel
    {
        [IndexField]
        public uint CreatureID { get; set; } = 0;
        public uint Idx { get; set; } = 0;
        public uint CreatureDisplayID { get; set; } = 0;
        public decimal DisplayScale { get; set; } = 1;
        public decimal Probability { get; set; } = 1;
        public int VerifiedBuild { get; set; } = -1;
    }

}
