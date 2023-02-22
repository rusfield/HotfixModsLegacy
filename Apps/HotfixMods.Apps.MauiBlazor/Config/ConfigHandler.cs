using HotfixMods.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HotfixMods.Apps.MauiBlazor.Config
{
    public static class ConfigHandler
    {
        public static AppConfig GetAppConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                var defaultAppConfig = new AppConfig();
                var serializer = new JsonSerializerOptions();
                serializer.WriteIndented = true;
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(defaultAppConfig, serializer));
            }

            var appConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(ConfigPath));
            appConfig.Save = () => Save(appConfig);
            appConfig.ConfigFilePath = ConfigPath.Replace("/config.json", "");
            return appConfig;
        }

        public static void Save(AppConfig appConfig)
        {
            var serializer = new JsonSerializerOptions();
            serializer.WriteIndented = true;
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(appConfig, serializer));
        }

        public static string ConfigPath
        {
            get
            {
                string mainDir = Path.Combine(FileSystem.Current.AppDataDirectory, "HotfixMods");
                Directory.CreateDirectory(mainDir);
                var configPath = $"{mainDir}/config.json";
                return configPath;
            }
        }
    }
}
