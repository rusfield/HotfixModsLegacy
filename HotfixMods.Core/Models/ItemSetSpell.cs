﻿using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class ItemSetSpell : IDb2, IHotfixesSchema
    {
        public int Id { get; set; }
        public int SpellId { get; set; }
        public int Threshold { get; set; }
        public int ItemSetId { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
