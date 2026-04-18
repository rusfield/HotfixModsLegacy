using HotfixMods.Core.Attributes;
using System.Reflection;

namespace HotfixMods.Apps.Console.Methods;

public static class HotfixSchemaTypeScanner
{
    public static List<Type> GetExternalClassesWithHotfixesSchema(Type type)
    {
        var externalClasses = new List<Type>();

        foreach (var property in type.GetProperties())
        {
            var propertyType = property.PropertyType;

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                propertyType = propertyType.GetGenericArguments()[0];
            }

            if (!propertyType.IsPrimitive && propertyType.Namespace != "System")
            {
                if (propertyType.GetCustomAttribute<HotfixesSchemaAttribute>() != null)
                {
                    externalClasses.Add(propertyType);
                }

                externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(propertyType));
            }
        }

        foreach (var innerType in type.GetNestedTypes())
        {
            if (innerType.IsClass && innerType.IsNestedPublic)
            {
                externalClasses.AddRange(GetExternalClassesWithHotfixesSchema(innerType));
            }
        }

        return externalClasses.Distinct().ToList();
    }
}
