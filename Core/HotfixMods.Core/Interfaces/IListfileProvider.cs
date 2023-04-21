namespace HotfixMods.Core.Interfaces
{
    public interface IListfileProvider
    {
        public Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() where TKey : notnull;
        public Task<Dictionary<TKey, string>> GetItemTexturesAsync<TKey>() where TKey : notnull;
        public Task<Dictionary<TKey, string>> GetModelFilesAsync<TKey>() where TKey : notnull;
    }
}
