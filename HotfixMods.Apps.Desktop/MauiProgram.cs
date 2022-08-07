using HotfixMods.Core.Providers;
using HotfixMods.Db2Provider.WowToolsFiles.Clients;
using HotfixMods.Infrastructure.Services;
using HotfixMods.MySqlProvider.EntityFrameworkCore.Clients;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace HotfixMods.Apps.Desktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.VisibleStateDuration = 2000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
            });

            // TODO: Cleanup 
            IDb2Provider db2Provider = new Db2Client();
            IMySqlProvider mySqlProvider = new MySqlClient("127.0.0.1","root","root","world","characters","hotfixes");
            builder.Services.AddSingleton(config =>
            {
                return new ItemService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -1200,
                    IdSize = 10.0,
                    IdRangeFrom = 5000000,
                    IdRangeTo = 6000000
                };
            });
            builder.Services.AddSingleton(config =>
            {
                return new CreatureService(db2Provider, mySqlProvider)
                {
                    VerifiedBuild = -2000,
                    IdSize = 5000.0,
                    IdRangeFrom = 100000000,
                    IdRangeTo = 999999999
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