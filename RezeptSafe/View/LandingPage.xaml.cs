using Rezeptbuch.Interfaces;
using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class LandingPage : ContentPage
{
    private ToolbarItem _themeButton;

    IPreferenceService _preferenceService;

    public LandingPage(LandingPageViewModel vm, IPreferenceService preferenceService)
	{
		InitializeComponent();

		this.BindingContext = vm;
        this._preferenceService = preferenceService;

        this._themeButton = new ToolbarItem
        {
            IconImageSource = "sun.svg",
            Command = new Command(() => { this.ToolbarItem_Clicked(this,EventArgs.Empty); })
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        this._themeButton.IconImageSource = (App.Current.UserAppTheme == AppTheme.Dark) ? "sun.svg" : "moon.svg";

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

            this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.AppTheme, App.Current.UserAppTheme.ToString());

            this._themeButton.IconImageSource = (App.Current.UserAppTheme == AppTheme.Dark) ? "sun.svg" : "moon.svg";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}