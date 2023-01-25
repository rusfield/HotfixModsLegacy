using HotfixMods.Core.Attributes;

namespace HotfixMods.Core.Models
{
    [WorldSchema]
    public class Gameobject
    {
        /*
         * Placeholder object for delete
         */

        [IndexField]
        public int Guid { get; set; }
        public int Id { get; set; }
    }
}
