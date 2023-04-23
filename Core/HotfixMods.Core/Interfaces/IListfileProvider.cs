namespace HotfixMods.Core.Interfaces
{
    public interface IListfileProvider
    {
        public Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() where TKey : notnull;
        public Task<Dictionary<TKey, string>> GetTexturesAsync<TKey>() where TKey : notnull;
        public Task<Dictionary<TKey, string>> GetModelsAsync<TKey>() where TKey : notnull;
    }
}
