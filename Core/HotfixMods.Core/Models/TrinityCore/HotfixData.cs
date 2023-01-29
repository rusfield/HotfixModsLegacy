﻿using HotfixMods.Core.Attributes;
using HotfixMods.Core.Enums;
using HotfixMods.Core.Interfaces;

namespace HotfixMods.Core.Models.TrinityCore
{
    [HotfixesSchema]
    public class HotfixData
    {
        public uint Id { get; set; }
        public uint UniqueId { get; set; }
        public uint TableHash { get; set; }
        public int RecordId { get; set; }
        public byte Status { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
