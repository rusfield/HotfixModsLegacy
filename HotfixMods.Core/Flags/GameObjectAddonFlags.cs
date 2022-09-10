using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Flags
{
    [Flags]
    public enum GameObjectAddonFlags
    {
        NONE = 0,
        IN_USE = 1,
        LOCKED = 2,
        INTERACT_COND = 4,
        TRANSPORT = 8,
        NOT_SELECTABLE = 16,
        NO_DESPAWN = 32,
        TRIGGERED = 64,
        DAMAGED = 512,
        DESTROYED = 1024
    }
}
