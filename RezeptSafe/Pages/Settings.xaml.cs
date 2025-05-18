namespace RezeptSafe.Pages;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        ThemeToggleButton.Text = App.Current.UserAppTheme == AppTheme.Dark
            ? "Wechsle zu Hellmodus"
            : "Wechsle zu Dunkelmodus";
    }

    private void OnToggleThemeClicked(object sender, EventArgs e)
    {
        App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
            ? AppTheme.Light
            : AppTheme.Dark;

        Preferences.Set("AppTheme",App.Current.UserAppTheme.ToString());

        UpdateButtonText();
    }
}