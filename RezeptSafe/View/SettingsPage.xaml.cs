using Rezeptbuch.Interfaces;
using Rezeptbuch.Services;
using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}