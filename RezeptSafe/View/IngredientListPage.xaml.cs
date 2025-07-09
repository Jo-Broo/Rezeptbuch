using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class IngredientListPage : ContentPage
{
	public IngredientListPage(IngredientListViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IngredientListViewModel vm)
            await vm.QueryAllIngredientsAsync();
    }
}