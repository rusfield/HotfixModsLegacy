﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models.TrinityCore
{
    [WorldSchema]
    public class CreatureModelInfo
    {
        public uint DisplayId { get; set; } = 0;
        public decimal BoundingRadius { get; set; } = 0;
        public decimal CombatReach { get; set; } = 0;
        public uint DisplayID_Other_Gender { get; set; } = 0;
        public int VerifiedBuild { get; set; } = -1;
    }

}
