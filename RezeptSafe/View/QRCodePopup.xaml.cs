using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Rezeptbuch.ViewModel;

namespace Rezeptbuch.View;

public partial class QRCodePopup : Popup
{
	public QRCodePopup()
	{
		InitializeComponent();
	}

	public void SetQRCodeValue(string value)
	{
		this.QRCodeImage.Value = value;
	}
}