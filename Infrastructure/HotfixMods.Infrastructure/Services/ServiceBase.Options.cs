namespace HotfixMods.Infrastructure.Services
{
    public partial class ServiceBase
    {
        protected async Task<Dictionary<TOptionKey, string>> GetOptionsAsync<TOptionKey>(string db2Name, string valueColumnName)
            where TOptionKey : notnull
        {
            return await GetOptionsAsync<TOptionKey, uint>(_appConfig.HotfixesSchema, db2Name, valueColumnName);
        }

        protected async Task<Dictionary<TOptionKey, string>> GetOptionsAsync<TOptionKey, TClientKey>(string schemaName, string db2Name, string valueColumnName)
            where TOptionKey : notnull
            where TClientKey : notnull
        {
            var results = new Dictionary<TOptionKey, string>();
            var options = await GetAsync(schemaName, db2Name, false, true);
            foreach (var option in options)
            {
                var key = option.Columns.Where(c => c.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()?.Value?.ToString();
                var value = option.Columns.Where(c => c.Name.Equals(valueColumnName, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault()?.Value?.ToString();

                if (key != null && value != null)
                {
                    var optionKey = (TOptionKey)Convert.ChangeType(key, typeof(TOptionKey));
                    results.Add(optionKey, value);
                }
            }
            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetEnumOptionsAsync<TOptionKey>(Type modelType, string propertyName)
            where TOptionKey : notnull
        {
            return await _serverEnumProvider.GetEnumValues<TOptionKey>(modelType, propertyName);
        }
    }
}
