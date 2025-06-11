using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RezeptSafe.Services;
using RezeptSafe.Model;
using RezeptSafe.ViewModel;
using RezeptSafe.View;
using RezeptSafe.Interfaces;

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
            builder.Services.AddSingleton<IRezeptService, LocalDataBase>();
            builder.Services.AddSingleton<IUserService, UserService>();
            // ViewModels
            builder.Services.AddSingleton<RecipesViewModel>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();
            builder.Services.AddTransient<CreateRecipeViewModel>();
            builder.Services.AddTransient<ProfilViewModel>();
            // Pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<CreateRecipePage>();
            builder.Services.AddTransient<ProfilPage>();

            return builder.Build();
        }
    }
}
