using HotfixMods.Infrastructure.DtoModels;

namespace HotfixMods.Infrastructure.Blazor.BlazorExtensions
{
    public static class IDtoExtensions
    {
        public static TValue? GetDtoValue<TValue>(this IDto dto)
         where TValue : class, new()
        {
            var dtoProperty = dto.GetType().GetProperty(typeof(TValue).Name);
            if (dtoProperty != null)
            {
                return (TValue?)dtoProperty?.GetValue(dto);
            }
            return null;
        }

        public static TValue? GetDtoGroupValue<TValue>(this IDto dto, Type groupType, int groupIndex)
            where TValue : class, new()
        {
            var groupProperty = dto.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && p.PropertyType.GetGenericArguments()[0] == groupType).FirstOrDefault();
            var getMethod = groupProperty?.PropertyType.GetMethod("get_Item");
            var group = groupProperty?.GetValue(dto);
            var count = (int?)group?.GetType()?.GetProperty("Count")?.GetValue(group);
            if (count != null && count > 0 && groupIndex < count)
            {
                var groupValue = getMethod?.Invoke(group, new object[] { groupIndex });
                return (TValue?)groupValue?.GetType()?.GetProperty(typeof(TValue).Name)?.GetValue(groupValue);
            }
            return null;
        }

        public static void SetDtoValueToDefault<TValue>(this IDto dto)
            where TValue : class, new()
        {
            var dtoProperty = dto.GetType().GetProperty(typeof(TValue).Name);
            dtoProperty?.SetValue(dto, Activator.CreateInstance(dtoProperty.PropertyType));
        }
    }
}
