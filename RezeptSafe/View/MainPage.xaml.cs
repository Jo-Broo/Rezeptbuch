using RezeptSafe.Services;
using RezeptSafe.Model;
using System.Collections.ObjectModel;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            InitializeTheme();
            this.BindingContext = viewModel;
        }

        void InitializeTheme()
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