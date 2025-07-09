using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class ProfilPage : ContentPage
{
	public ProfilPage(ProfilViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		(this.BindingContext as ProfilViewModel).NameHasChanged = true;
    }
}