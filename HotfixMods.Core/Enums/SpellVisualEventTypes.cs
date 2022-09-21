using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    public enum SpellVisualEventTypes
    {
        NONE = 0,
        PRECAST_START = 1,
        PRECAST_END = 2,
        CAST = 3,
        TRAVEL_START = 4,
        TRAVEL_END = 5,
        IMPACT = 6,
        AURA_START = 7,
        AURA_END = 8,
        AREA_TRIGGER_START = 9,
        AREA_TRIGGER_END = 10,
        CHANNEL_START = 11,
        CHANNEL_END = 12,
        ONE_SHOT = 13
    }
}
