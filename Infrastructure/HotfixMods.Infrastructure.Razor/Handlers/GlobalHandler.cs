
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.DtoModels;
using HotfixMods.Infrastructure.Razor.PageData;

namespace HotfixMods.Infrastructure.Razor.Handlers
{
    public static class GlobalHandler
    {
        public static Action<PageTab<IDto>>? LaunchTab;
        public static AppConfig? Config;

        static Dictionary<string, object> cache = new();

        public static T? GetFromCache<T>(string key)
        {
            if(cache.ContainsKey(key))
                return (T)cache[key];
            return default;
        }

        public static void AddToCache<T>(string key, T value)
        {
            if(Config != null && Config.CacheResults)
                cache.Add(key, value);
        }
    }
}
