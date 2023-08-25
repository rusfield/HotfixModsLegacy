﻿using MySqlConnector;

namespace HotfixMods.Providers.MySqlConnector.Client
{
    public partial class MySqlClient
    {
        string CSharpTypeToMySqlDataType(Type type)
        {
            return type.ToString() switch
            {
                "System.SByte" => "tinyint signed",
                "System.Int16" => "smallint signed",
                "System.Int32" => "int signed",
                "System.Int64" => "bigint signed",
                "System.Byte" => "tinyint unsigned",
                "System.UInt16" => "smallint unsigned",
                "System.UInt32" => "int unsigned",
                "System.UInt64" => "bigint unsigned",
                "System.String" => "text",
                "System.Decimal" => "float",
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        Type MySqlDataTypeToCSharpType(string type)
        {
            type = type.Split("(")[0]; // for varchar(100), etc.
            return type.ToString() switch
            {
                "tinyint" => typeof(sbyte),
                "tinyint signed" => typeof(sbyte),
                "tinyint unsigned" => typeof(byte),
                "smallint" => typeof(short),
                "smallint signed" => typeof(short),
                "smallint unsigned" => typeof(ushort),
                "int" => typeof(int),
                "int signed" => typeof(int),
                "int unsigned" => typeof(uint),
                "bigint" => typeof(long),
                "bigint signed" => typeof(long),
                "bigint unsigned" => typeof(ulong),
                "text" => typeof(string),
                "varchar" => typeof(string),
                "nvarchar" => typeof(string),
                "tinytext" => typeof(string),
                "mediumtext" => typeof(string),
                "longtext" => typeof(string),
                "float" => typeof(decimal),
                _ => throw new Exception($"{type} not implemented.")
            };
        }

        async Task<bool> CheckIfSchemaExistsAsync(string schemaName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @schemaName;", mySqlConnection);
            cmd.Parameters.AddWithValue("@schemaName", schemaName);
            var reader = await cmd.ExecuteReaderAsync();
            bool exists = false;
            while (reader.Read())
            {
                int count = reader.GetInt32(0);
                exists = count > 0;
            }
            await mySqlConnection.CloseAsync();
            return exists;
        }

        async Task<bool> CheckIfTableExistsAsync(string schemaName, string tableName)
        {
            using var mySqlConnection = new MySqlConnection(_connectionString);
            await mySqlConnection.OpenAsync();
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @schemaName AND table_name = @tableName;", mySqlConnection);
            cmd.Parameters.AddWithValue("schemaName", schemaName);
            cmd.Parameters.AddWithValue("tableName", tableName);
            var reader = await cmd.ExecuteReaderAsync();
            bool exists = false;
            while (reader.Read())
            {
                int count = reader.GetInt32(0);
                exists = count > 0;
            }
            await mySqlConnection.CloseAsync();
            return exists;
        }
    }
}
