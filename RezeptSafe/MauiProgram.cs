using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Rezeptbuch.Interfaces;
using Rezeptbuch.Services;
using Rezeptbuch.View;
using Rezeptbuch.ViewModel;
using ZXing.Net.Maui.Controls;

namespace Rezeptbuch
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
            builder.Services.AddTransient<IngredientListViewModel>();
            builder.Services.AddTransient<UtensilListViewModel>();
            builder.Services.AddTransient<UnitListViewModel>();
            builder.Services.AddTransient<AboutViewModel>();
            // Pages
            builder.Services.AddSingleton<LandingPage>();
            builder.Services.AddSingleton<ListRecipesPage>();
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<CreateRecipePage>();
            builder.Services.AddTransient<ProfilPage>();
            builder.Services.AddTransient<QRCodeScanner>();
            builder.Services.AddTransient<IngredientListPage>();
            builder.Services.AddTransient<UtensilListPage>();
            builder.Services.AddTransient<UnitListPage>();
            builder.Services.AddTransient<AboutPage>();

            return builder.Build();
        }
    }
}
