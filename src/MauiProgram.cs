using CatMatch.Services;
using CatMatch.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace CatMatch
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("LuckiestGuy-Regular.ttf", "LuckiestGuyRegular");
                    fonts.AddFont("WendyOne-Regular.ttf", "WendyOneRegular");
                })
                .UseMauiCommunityToolkit()
                .UseSkiaSharp();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<MainPage>();
            
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<ICatService, CatService>();
            builder.Services.AddSingleton<IScoreService, ScoreService>();

            return builder.Build();
        }
    }
}
