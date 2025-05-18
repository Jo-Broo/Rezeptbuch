using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RezeptSafe.Data;
using RezeptSafe.Model;
using RezeptSafe.Pages;

namespace RezeptSafe
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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<Settings>();
            builder.Services.AddTransient<CreateRecipe>();
            builder.Services.AddSingleton<IRecipeDatabase, RecipeDatabase>();
            builder.Services.AddSingleton<IUser, User>();

            return builder.Build();
        }
    }
}
