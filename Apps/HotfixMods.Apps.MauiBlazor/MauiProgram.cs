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
            var db2Client = new Db2Client("10.0.2.46259");
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
                    FromId = 10000,
                    ToId = 20000,
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
                    FromId = 10000,
                    ToId = 20000,
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

            /*
            // TODO: Cleanup 
            IDb2Provider db2Provider = new Db2Client();
            //IMySqlProvider mySqlProvider = new MySqlClient("127.0.0.1","root","root","world","characters","hotfixes");
            IMySqlProvider mySqlProvider = new MySqlProvider.Debug.Clients.MySqlProvider();

            builder.Services.AddSingleton(config =>
            {
                return new CreatureService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -2000,
                    IdSize = 5000,
                    IdRangeFrom = 100000000,
                    IdRangeTo = 200000000
                };
            });
            builder.Services.AddSingleton(config =>
            {
                return new SoundKitService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -800,
                    IdSize = 20,
                    IdRangeFrom = 1800000,
                    IdRangeTo = 1900000
                };
            });
            builder.Services.AddSingleton(config =>
            {
                return new AnimKitService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -700,
                    IdSize = 50,
                    IdRangeFrom = 60000,
                    IdRangeTo = 70000
                };
            });
            builder.Services.AddSingleton(config =>
            {
                return new SpellService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -1600,
                    IdSize = 50, 
                    IdRangeFrom = 2000000,
                    IdRangeTo = 3000000
                };
            });
            builder.Services.AddSingleton(config =>
            {
                return new SpellVisualKitService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -1700,
                    IdSize = 5, // TODO: Check
                    IdRangeFrom = 300000,
                    IdRangeTo = 400000
                };
            });
            */

            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
            });

            return builder.Build();
        }
    }
}