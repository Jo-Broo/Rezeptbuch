using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class LandingPage : ContentPage
{
    private ToolbarItem _themeButton;

    public LandingPage(LandingPageViewModel vm)
	{
		InitializeComponent();

		this.BindingContext = vm;

        this._themeButton = new ToolbarItem
        {
            Text = "+",
            Command = new Command(() => { this.ToolbarItem_Clicked(this,EventArgs.Empty); })
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        this._themeButton.Text = (App.Current.UserAppTheme == AppTheme.Dark) ? "☀️" : "🌙";

        this.ToolbarItems.Add(this._themeButton);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        this.ToolbarItems.Remove(this._themeButton);
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            App.Current.UserAppTheme = App.Current.UserAppTheme == AppTheme.Dark
            ? AppTheme.Light
            : AppTheme.Dark;

            Preferences.Set("AppTheme", App.Current.UserAppTheme.ToString());

            this._themeButton.Text = (App.Current.UserAppTheme == AppTheme.Dark) ? "☀️" : "🌙";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}