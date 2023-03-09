namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected async Task<Dictionary<TOptionKey, string>> GetOptionsAsync<TOptionKey, TClientKey>(string db2Name, string valueColumnName)
            where TOptionKey : notnull
            where TClientKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            var creatureTypes = await GetFromClientOnlyAsync(db2Name);
            foreach (var creatureType in creatureTypes)
            {
                var key = ((TClientKey?)creatureType.Columns.Where(c => c.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Value)?.ToString();
                var value = creatureType.Columns.Where(c => c.Name.Equals(valueColumnName, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault()?.Value?.ToString();

                if (key != null && value != null)
                {
                    var optionKey = (TOptionKey)Convert.ChangeType(key, typeof(TOptionKey));
                    results.Add(optionKey, value);
                }

            }
            return results;
        }
    }
}
