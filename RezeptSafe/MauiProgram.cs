using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using RezeptSafe.Interfaces;
using RezeptSafe.Services;
using RezeptSafe.View;
using RezeptSafe.ViewModel;

namespace RezeptSafe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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
            builder.Services.AddSingleton<IAlertService, AlertService>();
            // ViewModels
            builder.Services.AddSingleton<RecipesViewModel>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();
            builder.Services.AddTransient<CreateRecipeViewModel>();
            builder.Services.AddTransient<ProfilViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
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
