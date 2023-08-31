
using System.Text.Json.Serialization;

namespace HotfixMods.Infrastructure.Config
{
    public class AppConfig
    {
        
        public string HotfixesSchema { get; set; } = "hotfixes";
        public string CharactersSchema { get; set; } = "characters";
        public string WorldSchema { get; set; } = "world";
        public string Db2Path { get; set; } = @"\";
        public string DbdPath { get; set; } = @"\";
        public string TrinityCorePath { get; set; } = @"\";
        public string ListfilePath { get; set; } = @"\";
        public int HotfixDataTableFromId { get; set; } = 15000000;
        public int HotfixDataTableToId { get; set; } = 16000000;
        public string BuildInfo { get; set; } = "10.1.5.50469";
        public MySqlSettings MySql { get; set; } = new("127.0.0.1", "3306", "root", "root");
        public List<CustomRange> CustomRanges { get; set; } = new();


        public class MySqlSettings
        {
            public MySqlSettings(string server, string port,string username, string password)
            {
                Server = server;
                Port = port;
                Username = username;
                Password = password;
            }
            public string Server { get; set; }
            public string Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class CustomRange
        {
            public string Table { get; set; }
            public ulong FromId { get; set; }
            public ulong ToId { get; set; } 
        }

        // Set by whatever App/Frontend is handling the loading.
        [JsonIgnore]
        public Action? Save { get; set; }
        // Path to file if exist
        [JsonIgnore]
        public string? ConfigFilePath { get; set; }
        [JsonIgnore]
        public bool LoadedCorrectly { get; set; } = true;
        [JsonIgnore]
        public bool FirstLoad { get; set; } = false;
    }
}
