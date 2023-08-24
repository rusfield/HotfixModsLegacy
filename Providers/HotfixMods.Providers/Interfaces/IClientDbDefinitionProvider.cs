using HotfixMods.Providers.Models;

namespace HotfixMods.Providers.Interfaces
{
    public interface IClientDbDefinitionProvider
    {
        Task<DbRowDefinition?> GetDefinitionAsync(string db2Name);
        Task<IEnumerable<string>> GetAvailableDb2Async();
    }
}
