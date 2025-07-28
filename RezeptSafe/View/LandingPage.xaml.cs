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

        if (DeviceInfo.Platform == DevicePlatform.Android){
            this._themeButton = new ToolbarItem
            {
                IconImageSource = "sun.png",
                Command = new Command(() => { this.ToolbarItem_Clicked(this, EventArgs.Empty); })
            };
        }
        else
        {
            this._themeButton = new ToolbarItem
            {
                Text = "Theme",
                Command = new Command(() => { this.ToolbarItem_Clicked(this, EventArgs.Empty); })
            };
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if((App.Current?.UserAppTheme == AppTheme.Dark))
        {
            this._themeButton.IconImageSource = "sun.svg";
            this._themeButton.Text = "Lightmode";
        }
        else
        {
            this._themeButton.IconImageSource = "moon.svg";
            this._themeButton.Text = "Darkmode";
        }

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
            if(App.Current is not null)
            {
                App.Current.UserAppTheme = (App.Current.UserAppTheme == AppTheme.Dark)
                ? AppTheme.Light
                : AppTheme.Dark;

                this._preferenceService.SetPreference(Enum.RezeptbuchPreferences.AppTheme, App.Current.UserAppTheme.ToString());

                if ((App.Current.UserAppTheme == AppTheme.Dark))
                {
                    this._themeButton.IconImageSource = "sun.svg";
                    this._themeButton.Text = "Lightmode";
                }
                else
                {
                    this._themeButton.IconImageSource = "moon.svg";
                    this._themeButton.Text = "Darkmode";
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}