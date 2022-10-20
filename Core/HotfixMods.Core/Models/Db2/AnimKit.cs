using HotfixMods.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Models.Db2
{
    [HotfixesSchema]
    public class AnimKit
    {
        public int Id { get; set; } = -1;
        public int OneShotDuration { get; set; } = 0;
        public int OneShotStopAnimKitId { get; set; } = 0;
        public int LowDefAnimKitId { get; set; } = 0;
    }
}
