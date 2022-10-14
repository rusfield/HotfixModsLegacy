using HotfixMods.Models;

namespace HotfixMods.Interfaces
{
    public interface IServerDbDefinitionProvider
    {
        Task<DbRowDefinition> GetDefinitionsAsync();
    }
}
