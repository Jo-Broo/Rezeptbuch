using RezeptSafe.Interfaces;
using RezeptSafe.Services;
using RezeptSafe.ViewModel;

namespace RezeptSafe.View;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}