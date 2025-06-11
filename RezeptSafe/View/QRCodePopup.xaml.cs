namespace RezeptSafe.View;

public partial class QRCodePopup : ContentPage
{
	public QRCodePopup()
	{
		InitializeComponent();
	}

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}