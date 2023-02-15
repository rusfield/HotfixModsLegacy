using HotfixMods.Apps.MauiBlazor.Config;
using HotfixMods.Core.Interfaces;
using HotfixMods.Infrastructure.Blazor.Handlers;
using HotfixMods.Infrastructure.Config;
using HotfixMods.Infrastructure.Handlers;
//using HotfixMods.Infrastructure.Razor.Handlers;
using HotfixMods.Infrastructure.Services;
using HotfixMods.Providers.MySqlConnector.Client;
using HotfixMods.Providers.WowDev.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
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

            builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            builder.Services.AddSingleton<AppConfig>(provider =>
            {
                var appConfig = new AppConfig();
                config.Bind(appConfig);
                return appConfig;
            });

            builder.Services.AddSingleton<IClientDbProvider, Db2Client>(provider =>
            {
                return new Db2Client(config.GetValue<string>(nameof(AppConfig.BuildInfo)));
            });
            builder.Services.AddSingleton<IClientDbDefinitionProvider, Db2Client>(provider =>
            {
                return new Db2Client(config.GetValue<string>(nameof(AppConfig.BuildInfo)));
            });
            builder.Services.AddSingleton<IServerDbProvider, MySqlClient>(provider =>
            {
                var mySqlSection = config.GetSection(nameof(AppConfig.MySql));
                return new MySqlClient(
                    mySqlSection.GetValue<string>(nameof(AppConfig.MySql.Server)),
                    mySqlSection.GetValue<string>(nameof(AppConfig.MySql.Port)),
                    mySqlSection.GetValue<string>(nameof(AppConfig.MySql.Username)),
                    mySqlSection.GetValue<string>(nameof(AppConfig.MySql.Password))
                    );
            });
            builder.Services.AddSingleton<IServerDbDefinitionProvider, MySqlClient>(provider =>
            {
                var mySql = config.GetSection(nameof(AppConfig.MySql)).Value;
                return new MySqlClient(
                    config.GetValue<string>(nameof(AppConfig.MySql.Server)),
                    config.GetValue<string>(nameof(AppConfig.MySql.Port)),
                    config.GetValue<string>(nameof(AppConfig.MySql.Username)),
                    config.GetValue<string>(nameof(AppConfig.MySql.Password))
                    );
            });

            builder.Services.AddSingleton<GenericHotfixService>();
            builder.Services.AddSingleton<AnimKitService>();
            builder.Services.AddSingleton<GameobjectService>();
            builder.Services.AddSingleton<SoundKitService>();
            builder.Services.AddSingleton<ItemService>();
            builder.Services.AddSingleton<CreatureService>();
            builder.Services.AddSingleton<SpellService>();
            builder.Services.AddSingleton<SpellVisualKitService>();

            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
            });

            return builder.Build();
        }
    }
}