using HotfixMods.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotfixMods.Infrastructure.Business
{
    public static class Extensions
    {
        public static DbRow? EntityToDbRow<T>(this T? entity)
            where T : new()
        {
            if (null == entity)
                return null;

            var dbRow = new DbRow();
            foreach (var property in typeof(T).GetProperties())
            {
                dbRow.Columns.Add(new ()
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Value = property.GetValue(entity)!
                });
            }
            dbRow.Columns.Reverse();
            return dbRow;
        }

        public static DbRowDefinition? EntityToDbRowDefinition<T>(this T? entity)
            where T : new()
        {
            if (null == entity)
                return null;

            var dbRowDefinition = new DbRowDefinition();
            foreach (var property in typeof(T).GetProperties())
            {
                dbRowDefinition.ColumnDefinitions.Add(new()
                {
                    Name = property.Name,
                    Type = property.PropertyType
                });
            }
            dbRowDefinition.ColumnDefinitions.Reverse();
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
                var existingProperty = typeof(T).GetProperty(column.Name);
                if (existingProperty != null)
                    existingProperty.SetValue(entity, column.Value);
            }
            return entity;
        }

        public static string ToTableName<T>(this T entity)
            where T : new()
        {
            var type = typeof(T);

            // If there ever comes any exceptions, add them here
            return type.ToString() switch
            {
                _ => Regex.Replace(type.ToString(), @"(?<!_|^)([A-Z])", "_$1")
            };
        }
    }
}
