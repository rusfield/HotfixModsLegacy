using System.Reflection;

namespace HotfixMods.Core.Interfaces
{
    public interface IServerEnumProvider
    {
        public Task<Dictionary<TKey, string>> GetEnumValues<TKey>(Type? modelType, string propertyName)
            where TKey : notnull;
    }
}
