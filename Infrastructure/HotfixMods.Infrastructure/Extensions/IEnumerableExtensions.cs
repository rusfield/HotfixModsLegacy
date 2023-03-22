using System.Collections;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        // https://thomaslevesque.com/2019/11/18/using-foreach-with-index-in-c/
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        // https://stackoverflow.com/questions/30260858/async-await-using-linq-foreach
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            await Parallel.ForEachAsync(
                enumerable,
                async (item, _) => await action(item));
        }
    }
}
