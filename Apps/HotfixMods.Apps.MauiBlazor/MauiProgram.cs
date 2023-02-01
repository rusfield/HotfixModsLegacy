using HotfixMods.Apps.MauiBlazor.Config;
using HotfixMods.Infrastructure.Config;
//using HotfixMods.Infrastructure.Razor.Handlers;
using HotfixMods.Infrastructure.Services;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace HotfixMods.Apps.MauiBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var config = ConfigBuilder.Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            //builder.Configuration.AddConfiguration(config);
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.VisibleStateDuration = 2000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
            });

            var appConfig = config.Get<AppConfig>();
            var mySqlClient = new MySqlClient("127.0.0.1", "3306", "root", "root");
            var db2Client = new Db2Client(appConfig.BuildInfo, appConfig.GitHubAccessToken);
            //GlobalHandler.Config = appConfig;

            builder.Services.AddSingleton(config =>
            {
                return new GenericHotfixService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 1,
                    VerifiedBuild = -5501
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new AnimKitService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 10000,
                    VerifiedBuild = -5502
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new GameobjectService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 10000,
                    ToId = 20000,
                    VerifiedBuild = -5503
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new SoundKitService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 10000,
                    ToId = 20000,
                    VerifiedBuild = -5504
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new ItemService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 2000000,
                    ToId = 2100000,
                    VerifiedBuild = -5505
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new CreatureService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 3200000,
                    ToId = 3300000,
                    VerifiedBuild = -5506
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new SpellService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 5400000,
                    ToId = 5500000,
                    VerifiedBuild = -5507
                };
            });

            builder.Services.AddSingleton(config =>
            {
                return new SpellVisualKitService(mySqlClient, db2Client, mySqlClient, db2Client, appConfig)
                {
                    FromId = 10000,
                    ToId = 20000,
                    VerifiedBuild = -5508
                };
            });

            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
            });

            return builder.Build();
        }
    }
}