using HotfixMods.Core.Attributes;
using HotfixMods.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class DbRowExtensions
    {
        public static DbRow? EntityToDbRow<T>(this T? entity)
            where T : new()
        {
            if (null == entity)
                return null;

            var dbRow = new DbRow(typeof(T).Name);
            foreach (var property in typeof(T).GetProperties())
            {
                dbRow.Columns.Add(new()
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Value = property.GetValue(entity)!
                });
            }
            return dbRow;
        }

        public static IEnumerable<DbRow> EntitiesToDbRows<T>(this IEnumerable<T> entities)
            where T : new()
        {
            return entities.Where(e => e != null).Select(e => e.EntityToDbRow()!);
        }

        public static DbRowDefinition? EntityToDbRowDefinition<T>(this T? entity)
            where T : new()
        {
            if (null == entity)
                return null;

            return TypeToDbRowDefinition(typeof(T));
        }

        public static DbRowDefinition? TypeToDbRowDefinition(this Type? type)
        {
            if (null == type)
                return null;

            var dbRowDefinition = new DbRowDefinition(type.Name);
            foreach (var property in type.GetProperties())
            {
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = property.Name,
                    Type = property.PropertyType
                });
            }
            return dbRowDefinition;
        }

        public static T? DbRowToEntity<T>(this DbRow? dbRow)
            where T : new()
        {
            if (null == dbRow)
                return default;

            T entity = new();
            foreach (var column in dbRow.Columns)
            {
                try
                {
                    var existingProperty = typeof(T).GetProperties().Where(p => p.Name.Equals(column.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (existingProperty != null)
                        existingProperty.SetValue(entity, column.Value);
                }
                catch(Exception e)
                {
                    throw new Exception($"Unable to convert DbRow to {typeof(T).Name} for column {column.Name}: {e.Message}");
                }
            }
            return entity;
        }

        public static IEnumerable<T> DbRowsToEntities<T>(this IEnumerable<DbRow> dbRows)
            where T : new()
        {
            return dbRows.Where(d => d != null).Select(d => d.DbRowToEntity<T>()!);
        }

        public static string ToTableName<T>(this T entity)
            where T : new()
        {
            return typeof(T).Name.ToTableName();
        }

        public static int GetIdValue(this DbRow dbRow)
        {
            var idColumn = dbRow.Columns.FirstOrDefault(c => c.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            if (int.TryParse(idColumn?.Value?.ToString(), out int id) && id >= 0)
            {
                return id;
            }
            throw new Exception("DbRow does not contain a valid ID column.");
        }

        public static int GetIdValue<T>(this T entity)
            where T : new()
        {
            (string name, int value) = GetId(entity);
            return value;
        }

        public static string GetIdName<T>(this T entity) 
            where T : new()
        {
            (string name, int value) = GetId(entity);
            return name;
        }

        static (string, int) GetId<T>(T entity)
            where T : new()
        {
            var idAttributeProperties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(IndexFieldAttribute)));
            if (idAttributeProperties.Count() > 1)
                throw new Exception($"{typeof(T).Name} contains multiple ID attributes.");

            if (idAttributeProperties.Count() == 1)
            {
                if (int.TryParse(idAttributeProperties.First().GetValue(entity)?.ToString(), out int id) && id > 0)
                {
                    return (idAttributeProperties.First().Name, id);
                }
            }

            string defaultIdPropertyName = "Id";
            var idProperties = typeof(T).GetProperties().Where(p => p.Name.Equals(defaultIdPropertyName, StringComparison.InvariantCultureIgnoreCase));
            if (idProperties.Count() > 1)
                throw new Exception($"{typeof(T).Name} contains multiple {defaultIdPropertyName} properties.");

            if (idProperties.Count() == 1)
            {
                if (int.TryParse(idProperties.First().GetValue(entity)?.ToString(), out int id) && id > 0)
                {
                    return (defaultIdPropertyName, id);
                }
            }

            throw new Exception($"{typeof(T)} does not contain any {defaultIdPropertyName} properties.");
        }
    }
}
