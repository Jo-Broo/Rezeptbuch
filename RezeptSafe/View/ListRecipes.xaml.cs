using RezeptSafe.Services;
using RezeptSafe.Model;
using System.Collections.ObjectModel;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View
{
    public partial class ListRecipesPage : ContentPage
    {
        private ToolbarItem _addRecipeButton;

        public ListRecipesPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;

            this._addRecipeButton = new ToolbarItem
            {
                IconImageSource = "plus.svg",
                Command = new Command(async() => { await this.NavigateToCreateRecipeAsync(); })
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as RecipesViewModel)?.GetRecipesCommand.Execute(null);
            this.ToolbarItems.Add(this._addRecipeButton);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.ToolbarItems.Remove(this._addRecipeButton);
        }

        public async Task NavigateToCreateRecipeAsync()
        {
            await Shell.Current.GoToAsync(nameof(CreateRecipePage), true);
        }
    }
}