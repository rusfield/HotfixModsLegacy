using HotfixMods.Infrastructure.Extensions;

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

                if (key != null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        value = key;
                    else
                        value = $"{key} - {value}";


                    var optionKey = (TOptionKey)Convert.ChangeType(key, typeof(TOptionKey));
                    results.Add(optionKey, value);
                }
            }
            return results;
        }

        protected async Task<Dictionary<TOptionKey, string>> GetEnumOptionsAsync<TOptionKey>(Type modelType, string propertyName)
            where TOptionKey : notnull
        {
            var enumValues = await _serverEnumProvider.GetEnumValues<TOptionKey>(modelType, propertyName);
            var results = new Dictionary<TOptionKey, string>();
            if (!enumValues.ContainsKey(default(TOptionKey)))
                results.Add(default(TOptionKey), $"0 - None");

            for(int i = 0; i < enumValues.Count; i++)
            {
                var item = enumValues.ElementAt(i);
                results[item.Key] = $"{item.Key} - {item.Value}";
            }
            return results;
        }
    }
}
