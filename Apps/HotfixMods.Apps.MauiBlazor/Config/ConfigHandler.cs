using HotfixMods.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HotfixMods.Apps.MauiBlazor.Config
{
    public static class ConfigHandler
    {
        public static AppConfig GetAppConfig()
        {


            var appConfig = new AppConfig();

            try
            {
                if (!File.Exists(ConfigPath))
                {
                    var serializer = new JsonSerializerOptions();
                    serializer.WriteIndented = true;
                    File.WriteAllText(ConfigPath, JsonSerializer.Serialize(appConfig, serializer));
                    appConfig.FirstLoad = true;
                }

                appConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(ConfigPath));
            }
            catch(Exception ex)
            {
                appConfig.LoadedCorrectly = false;
            }

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
