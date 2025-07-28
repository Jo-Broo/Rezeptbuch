using Rezeptbuch.Services;
using Rezeptbuch.Model;
using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;


public partial class CreateRecipePage : ContentPage
{
    private ToolbarItem _qrScannerButton;

    public CreateRecipePage(CreateRecipeViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;

        if(DeviceInfo.Platform == DevicePlatform.Android)
        {
            this._qrScannerButton = new ToolbarItem
            {
                IconImageSource = "qrcode.svg",
                Command = vm.ScanQRCodeClickedCommand
            };
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var vm = (BindingContext as CreateRecipeViewModel);

        vm?.InitializeCommand.Execute(null);

        if (!this.ToolbarItems.Contains(this._qrScannerButton) && DeviceInfo.Platform == DevicePlatform.Android)
        {
            this.ToolbarItems.Add(this._qrScannerButton);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        this.ToolbarItems.Remove(this._qrScannerButton);
    }

    private void OnIngredientsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            foreach (Ingredient removed in e.PreviousSelection.Cast<Ingredient>())
            {
                removed.IsSelected = false;
            }

            foreach (Ingredient added in e.CurrentSelection.Cast<Ingredient>())
            {
                added.IsSelected = true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    private void OnUtensilsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            foreach (Utensil removed in e.PreviousSelection.Cast<Utensil>())
            {
                removed.IsSelected = false;
            }

            foreach (Utensil added in e.CurrentSelection.Cast<Utensil>())
            {
                added.IsSelected = true;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}