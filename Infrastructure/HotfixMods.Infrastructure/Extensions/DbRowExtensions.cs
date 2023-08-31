﻿using HotfixMods.Providers.Models;

namespace HotfixMods.Infrastructure.Extensions
{
    public static class DbRowExtensions
    {
        public static DbRow? EntityToDbRow<T>(this T? entity, DbRowDefinition definition)
            where T : new()
        {
            if (null == entity)
                return null;

            var dbRow = new DbRow(typeof(T).Name);
            foreach (var property in typeof(T).GetProperties())
            {
                var dbColumn = definition.ColumnDefinitions.First(c => c.Name == property.Name);
                dbRow.Columns.Add(new()
                {
                    Definition = dbColumn,
                    Value = property.GetValue(entity)!
                });
            }
            return dbRow;
        }

        public static T GetValueByNameAs<T>(this DbRow dbRow, string columnName)
        {
            var column = dbRow.Columns.FirstOrDefault(c => c.Definition.Name.Equals(columnName, StringComparison.InvariantCultureIgnoreCase));
            if(column != null)
            {
                return (T)Convert.ChangeType(column.Value, typeof(T));
            }
            throw new Exception($"Unable to get {columnName} from DbRow {dbRow.Db2Name}.");
        }

        public static IEnumerable<DbRow> EntitiesToDbRows<T>(this IEnumerable<T> entities, DbRowDefinition definition)
            where T : new()
        {
            return entities.Where(e => e != null).Select(e => e.EntityToDbRow(definition)!);
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
                    var existingProperty = typeof(T).GetProperties().Where(p => p.Name.Equals(column.Definition.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (existingProperty != null)
                        existingProperty.SetValue(entity, column.Value);
                }
                catch(Exception e)
                {
                    throw new Exception($"Unable to convert DbRow to {typeof(T).Name} for column {column.Definition.Name}: {e.Message}");
                }
            }
            return entity;
        }

        public static List<T> DbRowsToEntities<T>(this PagedDbResult pagedDbResult)
            where T : new()
        {
            return pagedDbResult.Rows.Select(d => d.DbRowToEntity<T>()!).ToList();
        }

        public static PagedDbResult<T> DbRowsToPagedEntities<T>(this PagedDbResult pagedDbResult)
            where T : new()
        {
            var result = new PagedDbResult<T>(pagedDbResult.PageIndex, pagedDbResult.PageSize, pagedDbResult.TotalRowCount)
            {
                Rows = pagedDbResult.Rows.Select(d => d.DbRowToEntity<T>()!).ToList()
            };

            return result;
        }

        public static string GetIdColumnName(this DbRow row)
        {
            return row.GetIdColumn().Definition.Name;
        }

        public static object GetIdColumnValue(this DbRow row)
        {
            return row.GetIdColumn().Value;
        }

        public static DbColumn GetIdColumn(this DbRow row)
        {
            return row.Columns.Where(p => p.Definition.IsIndex).First();
        }
    }
}
