namespace HotfixMods.Providers.Interfaces
{
    public interface IServerValuesProvider
    {
        public Task<Dictionary<TKey, string>> GetServerValuesAsync<TKey>(Type? modelType, string propertyName)
            where TKey : notnull;
    }
}
