using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using RezeptSafe.Interfaces;
using RezeptSafe.Services;
using RezeptSafe.View;
using RezeptSafe.ViewModel;
using ZXing.Net.Maui.Controls;

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
                    fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FontAwesomeBrands");
                    fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FontAwesomeRegular");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesomeSolid");
                })
                .UseBarcodeReader();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Service
            builder.Services.AddSingleton<IRezeptService, LocalDataBase>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<IRezeptShareService, RezeptShareService>();
            builder.Services.AddSingleton<IPreferenceService, PreferenceService>();
            // ViewModels
            builder.Services.AddSingleton<RecipesViewModel>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();
            builder.Services.AddTransient<CreateRecipeViewModel>();
            builder.Services.AddTransient<ProfilViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<LandingPageViewModel>();
            // Pages
            builder.Services.AddSingleton<LandingPage>();
            builder.Services.AddSingleton<ListRecipesPage>();
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<CreateRecipePage>();
            builder.Services.AddTransient<ProfilPage>();
            builder.Services.AddTransient<QRCodeScanner>();

            return builder.Build();
        }
    }
}
