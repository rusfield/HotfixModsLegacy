using HotfixMods.Core.Models.Db2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.DtoModels
{
    public class AnimKitDto
    {
        public AnimKit AnimKit { get; set; } = new();
        public IEnumerable<AnimKitSegment> AnimKitSegments { get; set; } = new List<AnimKitSegment>();
    }
}
