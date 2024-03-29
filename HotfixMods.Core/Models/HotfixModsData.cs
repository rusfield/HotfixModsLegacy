﻿using HotfixMods.Core.Enums;
using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    public class HotfixModsData : IHotfixesSchema
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int RecordId { get; set; }
        public string Comment { get; set; }
        public int VerifiedBuild { get; set; }
    }
}
