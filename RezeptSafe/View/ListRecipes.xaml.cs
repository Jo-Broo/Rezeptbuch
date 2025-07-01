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
                Command = viewModel.GoToCreateRecipeCommand
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((RecipesViewModel)this.BindingContext)?.GetRecipesCommand.Execute(null);

            if (!this.ToolbarItems.Contains(this._addRecipeButton))
            {
                this.ToolbarItems.Add(this._addRecipeButton);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.ToolbarItems.Remove(this._addRecipeButton);
        }
    }
}