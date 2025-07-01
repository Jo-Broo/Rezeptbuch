using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class DetailsPage : ContentPage
{
    private ToolbarItem _deleteRecipeButton;

    public DetailsPage(RecipeDetailsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;

        this._deleteRecipeButton = new ToolbarItem
        {
            IconImageSource = "trash.svg",
            Command = viewModel.DeleteRecipeCommand
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!this.ToolbarItems.Contains(this._deleteRecipeButton))
        {
            this.ToolbarItems.Add(this._deleteRecipeButton);
        }
    }

    protected override void OnDisappearing() 
    { 
        base.OnDisappearing(); 

        this.ToolbarItems.Remove(this._deleteRecipeButton);
    }
}