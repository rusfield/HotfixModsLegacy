using HotfixMods.Core.Models;

namespace HotfixMods.Core.Interfaces
{
    public interface IClientDbDefinitionProvider
    {
        Task<DbRowDefinition> GetDefinitionsAsync(string location, string db2Name);
    }
}
