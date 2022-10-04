using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotfixMods.Providers.MySql.MySqlConnector.Client
{
    public class MySqlClient
    {
        string _server;
        string _user;
        string _password;
        string _worldSchemaName;
        string _charactersSchemaName;
        string _hotfixesSchemaName;

        MySqlConnection _mySqlConnection;

        public MySqlClient(string server, string user, string password, string worldSchemaName, string charactersSchemaName, string hotfixesSchemaName)
        {
            _server = server;
            _user = user;
            _password = password;
            _worldSchemaName = worldSchemaName;
            _charactersSchemaName = charactersSchemaName;
            _hotfixesSchemaName = hotfixesSchemaName;
        }

        public async Task<Dictionary<string, object>> RawGetQueryAsync(Dictionary<string, Type> definitions, string tableName)
        {
            var result = new Dictionary<string, object>();
            var query = $"SELECT * FROM {tableName};";

            using var command = new MySqlCommand(query, _mySqlConnection);
            using var reader = command.ExecuteReader();

            for (int i = 0; i<definitions.Count; i++)
            {
                string fieldName = definitions.ElementAt(i).Key;

                
                object fieldProperty = definitions.ElementAt(i).Value switch
                {
                    typeof(int) => reader.GetInt32(i),
                    typeof(uint) => reader.GetUInt32(i),
                    _ => throw new Exception("Not implemented")
                };


                //var type = typeof(uint);
                int test = type switch
                {
                    int t1 => 1,
                    typeof(uint) => 2,
                    _ => -1
                };

                var type = typeof(uint);
                int test;
                switch (type)
                {
                    case int t1:
                        test = 1;
                        break;
                }

            }
        }
    }
}
