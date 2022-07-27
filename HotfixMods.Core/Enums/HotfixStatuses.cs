using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Core.Enums
{
    public enum HotfixStatuses
    {
        VALID = 1,          // Use this to add something to DB2
        RECORD_REMOVED = 2, // Use this to remove something from DB2
        INVALID = 3,        // Use this to remove an existing hotfix (?)
        NOT_PUBLIC = 4,     // ?
    }
}
