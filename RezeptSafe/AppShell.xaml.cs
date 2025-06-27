using RezeptSafe.View;

namespace RezeptSafe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // nameof(DetailsPage) == "DetailsPage"

            #region Pages
            Routing.RegisterRoute(nameof(CreateRecipePage), typeof(CreateRecipePage));
            Routing.RegisterRoute(nameof(ListRecipesPage), typeof(ListRecipesPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ProfilPage), typeof(ProfilPage));
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            #endregion
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string theme = Preferences.Get("AppTheme", "Light");
            App.Current.UserAppTheme = theme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Light
            };

            this.ThemeButton.Text = (App.Current.UserAppTheme == AppTheme.Dark) ? "☀️" : "🌙";
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

                Preferences.Set("AppTheme", App.Current.UserAppTheme.ToString());

                this.ThemeButton.Text = (App.Current.UserAppTheme == AppTheme.Dark) ? "☀️" : "🌙";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
