using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(RecipeDetailsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}