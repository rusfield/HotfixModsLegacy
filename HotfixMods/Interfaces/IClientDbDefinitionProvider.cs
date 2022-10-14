using HotfixMods.Models;

namespace HotfixMods.Interfaces
{
    public interface IClientDbDefinitionProvider
    {
        Task<DbRowDefinition> GetDefinitionsAsync();
    }
}
