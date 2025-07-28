using Rezeptbuch.Services;
using Rezeptbuch.Model;
using System.Collections.ObjectModel;
using Rezeptbuch.ViewModel;
using System.Threading.Tasks;

namespace Rezeptbuch.View
{
    public partial class ListRecipesPage : ContentPage
    {
        private ToolbarItem _addRecipeButton;

        public ListRecipesPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;

            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                this._addRecipeButton = new ToolbarItem
                {
                    IconImageSource = "plus.svg",
                    Command = viewModel.GoToCreateRecipeCommand
                };
            }
            else
            {
                this._addRecipeButton = new ToolbarItem
                {
                    Text = "Erstellen",
                    Command = viewModel.GoToCreateRecipeCommand
                };
            }
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

        public async Task Test()
        {
            var vm = this.BindingContext as RecipesViewModel;

            if(vm is not null)
            {
                await vm._alertService.ShowAlertAsync("Test", "Test");
            }
        }
    }
}