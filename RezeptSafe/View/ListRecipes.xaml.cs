using RezeptSafe.Services;
using RezeptSafe.Model;
using System.Collections.ObjectModel;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View
{
    public partial class ListRecipesPage : ContentPage
    {
        private ToolbarItem addRecipeButton;

        public ListRecipesPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            InitializeTheme();
            this.BindingContext = viewModel;

            this.addRecipeButton = new ToolbarItem
            {
                Text = "+",
                Command = new Command(async() => { await this.NavigateToCreateRecipeAsync(); })
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as RecipesViewModel)?.GetRecipesCommand.Execute(null);
            this.ToolbarItems.Add(this.addRecipeButton);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.ToolbarItems.Remove(this.addRecipeButton);
        }

        public async Task NavigateToCreateRecipeAsync()
        {
            await Shell.Current.GoToAsync(nameof(CreateRecipePage), true);
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