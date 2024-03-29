﻿using HotfixMods.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models
{
    [HotfixesSchema]
    public class ChrCustomizationCategory
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public int CustomizeIcon { get; set; }
        public int CustomizeIconSelected { get; set; }
        public int OrderIndex { get; set; }
        public int CameraZoomLevel { get; set; }
        public int Flags { get; set; }
        public int SpellShapeshiftFormID { get; set; }
        public decimal CameraDistanceOffset { get; set; }
        public int VerifiedBuild { get; set; }
    }

}
