namespace HotfixMods.Core.Interfaces
{
    public interface IListfileProvider
    {
        public Task<Dictionary<TKey, string>> GetIconsAsync<TKey>() where TKey : notnull;
    }
}
