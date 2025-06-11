using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class ProfilPage : ContentPage
{
	public ProfilPage(ProfilViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}