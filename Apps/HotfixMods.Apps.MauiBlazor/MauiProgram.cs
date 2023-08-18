using HotfixMods.Apps.MauiBlazor.Config;
using HotfixMods.Core.Interfaces;
using HotfixMods.Infrastructure.Blazor.Handlers;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Handlers;
//using HotfixMods.Infrastructure.Razor.Handlers;
using HotfixMods.Infrastructure.Services;
using HotfixMods.Providers.Listfile.Client;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.TrinityCore.Client;
using HotfixMods.Providers.WowDev.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace HotfixMods.Apps.MauiBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var appConfig = ConfigHandler.GetAppConfig();

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

            builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            builder.Services.AddSingleton(provider =>
            {
                return appConfig;
            });

            builder.Services.AddSingleton<IClientDbProvider, Db2Client>(provider =>
            {
                return new Db2Client(appConfig.BuildInfo);
            });
            builder.Services.AddSingleton<IClientDbDefinitionProvider, Db2Client>(provider =>
            {
                return new Db2Client(appConfig.BuildInfo);
            });
            builder.Services.AddSingleton<IServerDbProvider, MySqlClient>(provider =>
            {
                return new MySqlClient(
                    appConfig.MySql.Server,
                    appConfig.MySql.Port,
                    appConfig.MySql.Username,
                    appConfig.MySql.Password
                    );
            });
            builder.Services.AddSingleton<IServerDbDefinitionProvider, MySqlClient>(provider =>
            {
                return new MySqlClient(
                    appConfig.MySql.Server,
                    appConfig.MySql.Port,
                    appConfig.MySql.Username,
                    appConfig.MySql.Password
                    );
            });
            builder.Services.AddSingleton<IServerEnumProvider, TrinityCoreClient>(provider =>
            {
                return new TrinityCoreClient(appConfig.TrinityCorePath)
                {
                    CacheResults = appConfig.CacheFileResults
                };
            });
            builder.Services.AddSingleton<IListfileProvider, ListfileClient>(provider =>
            {
                return new ListfileClient(appConfig.ListfilePath)
                {
                    CacheResults = appConfig.CacheFileResults
                };
            });

            builder.Services.AddSingleton<HotfixService>();
            builder.Services.AddSingleton<AnimKitService>();
            builder.Services.AddSingleton<GameobjectService>();
            builder.Services.AddSingleton<SoundKitService>();
            builder.Services.AddSingleton<ItemService>();
            builder.Services.AddSingleton<CreatureService>();
            builder.Services.AddSingleton<SpellService>();
            builder.Services.AddSingleton<SpellVisualKitService>();

            builder.Services.AddBlazorWebViewDeveloperTools();


            return builder.Build();
        }
    }
}