namespace RezeptSafe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetAppThemeFromPreferences();

            MainPage = new AppShell();
        }

        private void SetAppThemeFromPreferences()
        {
            string theme = Preferences.Get("AppTheme", "Light");

            App.Current.UserAppTheme = theme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Light
            };
        }
    }
}