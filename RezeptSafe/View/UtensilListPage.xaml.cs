using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class UtensilListPage : ContentPage
{
	public UtensilListPage(UtensilListViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UtensilListViewModel vm)
            await vm.QueryAllUtensilsAsync();
    }
}