using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class LandingPage : ContentPage
{
	public LandingPage(LandingPageViewModel vm)
	{
		InitializeComponent();

		this.BindingContext = vm;
	}
}