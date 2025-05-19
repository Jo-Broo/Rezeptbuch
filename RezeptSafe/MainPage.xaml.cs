using RezeptSafe.Data;
using RezeptSafe.Pages;
using RezeptSafe.Model;
using System.Collections.ObjectModel;

namespace RezeptSafe
{
    public partial class MainPage : ContentPage
    {
        private readonly IRecipeDatabase database;
        private readonly IUser user;
        private ObservableCollection<Recipe> _recipes = new();

        public MainPage(IRecipeDatabase db, IUser user)
        {
            InitializeComponent();
            string theme = Preferences.Get("AppTheme", "Light");
            App.Current.UserAppTheme = theme switch
            {
                "Dark" => AppTheme.Dark,
                "Light" => AppTheme.Light,
                _ => AppTheme.Light
            };
            
            this.database = db;
            this.user = user;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var items = await this.database.GetAllRecipesAsync();
            _recipes.Clear();
            foreach (var recipe in items)
            {
                recipe.Ingredients = await this.database.GetIngredientsForRecipeAsync(recipe.Id);
                recipe.Utensils = await this.database.GetUtensilsForRecipeAsync(recipe.Id);
                _recipes.Add(recipe);
            }

            this.RecipeList.ItemsSource = _recipes;
        }

        private async void OpenMenu(object sender, EventArgs e)
        {
            this.MyButton.IsVisible = false;
            Overlay.IsVisible = true;
            await Overlay.FadeTo(1, 200);
            await SideMenu.TranslateTo(0, 0, 250, Easing.CubicOut);
        }

        private async void CloseMenu(object sender, EventArgs e)
        {
            this.MyButton.IsVisible = true;
            await SideMenu.TranslateTo(250, 0, 250, Easing.CubicIn);
            await Overlay.FadeTo(0, 200);
            Overlay.IsVisible = false;
        }

        private async void NavigateSettings(object sender, EventArgs e)
        {
            await SideMenu.TranslateTo(250, 0, 200); // Menü schließen
            Overlay.IsVisible = false;

            await Navigation.PushAsync(new Settings(this.database));
        }

        private async void NavigateCreateRecipe(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateRecipe(this.database, this.user));
        }

    }

}