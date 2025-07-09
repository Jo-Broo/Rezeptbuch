using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class UnitListPage : ContentPage
{
	public UnitListPage(UnitListViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UnitListViewModel vm)
            await vm.QueryAllUnitsAsync();
    }
}