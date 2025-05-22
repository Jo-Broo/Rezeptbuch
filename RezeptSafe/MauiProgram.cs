using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RezeptSafe.Services;
using RezeptSafe.Model;
using RezeptSafe.ViewModel;
using RezeptSafe.View;

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

            // Service
            builder.Services.AddSingleton<IRezeptService, LocalDatabaseService>();
            //builder.Services.AddSingleton<IUser, UserService>();
            // ViewModels
            builder.Services.AddSingleton<RecipesViewModel>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();
            // Pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();
            //builder.Services.AddTransient<Settings>();
            //builder.Services.AddTransient<CreateRecipe>();

            return builder.Build();
        }
    }
}
