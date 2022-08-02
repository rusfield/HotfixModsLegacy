using HotfixMods.Apps.Desktop.Data;
using HotfixMods.Core.Providers;
using HotfixMods.Db2Provider.WowToolsFiles.Clients;
using HotfixMods.Infrastructure.Services;
using HotfixMods.MySqlProvider.EntityFrameworkCore.Clients;
using Microsoft.AspNetCore.Components.WebView.Maui;
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


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            return builder.Build();
        }
    }
}