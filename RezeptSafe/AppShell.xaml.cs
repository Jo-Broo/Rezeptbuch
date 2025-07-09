using RezeptSafe.View;

namespace RezeptSafe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            #region Pages
            // nameof(DetailsPage) == "DetailsPage"
            Routing.RegisterRoute(nameof(CreateRecipePage), typeof(CreateRecipePage));
            Routing.RegisterRoute(nameof(ListRecipesPage), typeof(ListRecipesPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ProfilPage), typeof(ProfilPage));
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(QRCodeScanner), typeof(QRCodeScanner));
            Routing.RegisterRoute(nameof(IngredientListPage), typeof(IngredientListPage));
            Routing.RegisterRoute(nameof(UtensilListPage), typeof(UtensilListPage));
            Routing.RegisterRoute(nameof(UnitListPage), typeof(UnitListPage));
            #endregion
        }
    }
}
