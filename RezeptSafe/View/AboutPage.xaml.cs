using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}