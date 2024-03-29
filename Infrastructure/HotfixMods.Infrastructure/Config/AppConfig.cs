﻿
using System.Text.Json.Serialization;

namespace HotfixMods.Infrastructure.Config
{
    public class AppConfig
    {
        
        public string HotfixesSchema { get; set; } = "hotfixes";
        public string CharactersSchema { get; set; } = "characters";
        public string WorldSchema { get; set; } = "world";
        public string Db2Path { get; set; } = @"\";
        public string TrinityCorePath { get; set; } = @"\";
        public string ListfilePath { get; set; } = @"\";
        public int HotfixDataTableFromId { get; set; } = 15000000;
        public int HotfixDataTableToId { get; set; } = 16000000;
        public string BuildInfo { get; set; } = "10.1.5.50469";
        public bool CacheFileResults { get; set; } = true;
        public MySqlSettings MySql { get; set; } = new("127.0.0.1", "3306", "root", "root");
        public ServiceSettings GenericHotfixSettings { get; set; } = new(0, 0, -5501);
        public ServiceSettings AnimKitSettings { get; set; } = new(60000, 63000, -5502);
        public ServiceSettings GameobjectSettings { get; set; } = new(400000, 900000, -5503);
        public ServiceSettings SoundKitSettings { get; set; } = new(1100000, 1200000, -5504);
        public ServiceSettings ItemSettings { get; set; } = new(2000000, 2100000, -5505);
        public ServiceSettings CreatureSettings { get; set; } = new(3200000, 3300000, -5506);
        public ServiceSettings SpellSettings { get; set; } = new(5400000, 5500000, -5507);
        public ServiceSettings SpellVisualKitSettings { get; set; } = new(900000, 910000, -5508);

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

        public class ServiceSettings
        {
            public ServiceSettings(int fromId, int toId, int verifiedBuild)
            {
                FromId = fromId;
                ToId = toId;
                VerifiedBuild = verifiedBuild;
            }

            public int FromId { get; set; }
            public int ToId { get; set; }
            public int VerifiedBuild { get; set; }
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
