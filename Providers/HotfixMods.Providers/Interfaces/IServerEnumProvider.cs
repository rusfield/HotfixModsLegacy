namespace HotfixMods.Providers.Interfaces
{
    public interface IServerEnumProvider
    {
        public Task<Dictionary<TKey, string>> GetEnumValues<TKey>(Type? modelType, string propertyName)
            where TKey : notnull;
    }
}
