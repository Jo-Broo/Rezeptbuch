using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class DetailsPage : ContentPage
{
    private ToolbarItem _deleteRecipeButton;
    private ToolbarItem _editRecipeButton;

    public DetailsPage(RecipeDetailsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

        if(DeviceInfo.Platform == DevicePlatform.Android)
        {
            this._deleteRecipeButton = new ToolbarItem
            {
                IconImageSource = "trash.svg",
                Command = viewModel.DeleteRecipeCommand
            };

            this._editRecipeButton = new ToolbarItem
            {
                IconImageSource = "pen.svg",
                Command = viewModel.NavigateToEditRecipeCommand
            };
        }
        else
        {
            this._deleteRecipeButton = new ToolbarItem
            {
                Text = "Löschen",
                Command = viewModel.DeleteRecipeCommand
            };

            this._editRecipeButton = new ToolbarItem
            {
                Text = "Bearbeiten",
                Command = viewModel.NavigateToEditRecipeCommand
            };
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!this.ToolbarItems.Contains(this._deleteRecipeButton))
        {
            this.ToolbarItems.Add(this._deleteRecipeButton);
        }

        if (!this.ToolbarItems.Contains(this._editRecipeButton))
        {
            this.ToolbarItems.Add(this._editRecipeButton);
        }
    }

    protected override void OnDisappearing() 
    { 
        base.OnDisappearing(); 

        this.ToolbarItems.Clear();
    }
}