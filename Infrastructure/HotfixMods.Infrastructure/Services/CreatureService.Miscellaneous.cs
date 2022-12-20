using HotfixMods.Core.Models;
using HotfixMods.Core.Models.Db2;


namespace HotfixMods.Infrastructure.Services
{
    public partial class CreatureService
    {
        Dictionary<int, Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> customizationCache = new();

        public async Task<Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>> GetCustomizationOptions(int chrModelId)
        {
            if (customizationCache.ContainsKey(chrModelId))
                return customizationCache[chrModelId];

            var result = new Dictionary<ChrCustomizationOption, List<ChrCustomizationChoice>>();

            var options = await GetAsync<ChrCustomizationOption>(new DbParameter(nameof(ChrCustomizationOption.ChrModelId), chrModelId));
            foreach (var option in options)
            {
                var choices = await GetAsync<ChrCustomizationChoice>(new DbParameter(nameof(ChrCustomizationChoice.ChrCustomizationOptionId), option.Id));
                result.Add(option, choices);
            }

            // In case it has been added elsewhere in the meantime
            if (!customizationCache.ContainsKey(chrModelId) && result.Count > 0)
                customizationCache.Add(chrModelId, result);
            return result;
        }
    }
}
