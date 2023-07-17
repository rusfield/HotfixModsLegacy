﻿using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{
    [WorldSchema]
    public class Gameobject
    {
        /*
         * Placeholder object for delete
         */

        [IndexField]
        public ulong Guid { get; set; }
        public int ID { get; set; }
    }
}
