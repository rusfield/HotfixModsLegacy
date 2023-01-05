using HotfixMods.Infrastructure.DtoModels;
using System.Collections;

namespace HotfixMods.Infrastructure.Extensions
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

        public static IList GetDtoGroup(this IDto dto, Type groupType)
        {
            var properties = dto.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && property.PropertyType.GetGenericArguments()[0] == groupType)
                {
                    return (IList)property.GetValue(dto);
                }
            }
            throw new Exception($"{dto.GetType().Name} does not have any lists of type {groupType.Name}.");
        }

        public static void SetDtoValueToDefault<TValue>(this IDto dto)
            where TValue : class, new()
        {
            var dtoProperty = dto.GetType().GetProperty(typeof(TValue).Name);
            dtoProperty?.SetValue(dto, Activator.CreateInstance(dtoProperty.PropertyType));
        }
    }
}
