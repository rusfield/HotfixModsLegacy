namespace HotfixMods.Infrastructure.Services
{
    public partial class FactionService
    {
        public async Task<Dictionary<ushort, string>> GetFactionIdOptionsAsync()
        {
            return await GetDb2OptionsAsync<ushort>("Faction", "Name");
        }
    }
}
