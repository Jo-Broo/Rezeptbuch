using RezeptSafe.Pages;

namespace RezeptSafe
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            string theme = Preferences.Get("AppTheme", "Light");
            App.Current.UserAppTheme = theme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Light
            };
        }

        private async void OnOpenMenuClicked(object sender, EventArgs e)
        {
            Overlay.IsVisible = true;
            await Overlay.FadeTo(1, 200);
            await SideMenu.TranslateTo(0, 0, 250, Easing.CubicOut);
        }

        private async void OnCloseMenuClicked(object sender, EventArgs e)
        {
            await SideMenu.TranslateTo(250, 0, 250, Easing.CubicIn);
            await Overlay.FadeTo(0, 200);
            Overlay.IsVisible = false;
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await SideMenu.TranslateTo(250, 0, 200); // Menü schließen
            Overlay.IsVisible = false;

            await Navigation.PushAsync(new Settings());
        }

    }

}
