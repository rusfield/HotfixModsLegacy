using System.Collections;

namespace HotfixMods.Infrastructure.Extensions
{
    public static  class IListExtensions
    {
        public static void MoveElement(this IList list, int oldIndex, int newIndex)
        {
            if (oldIndex >= 0 && oldIndex < list.Count && newIndex >= 0 && newIndex < list.Count)
            {
                var item = list[oldIndex];
                list.RemoveAt(oldIndex);
                list.Insert(newIndex, item);
            }
        }
    }
}
