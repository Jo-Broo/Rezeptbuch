using CommunityToolkit.Maui.Views;
using RezeptSafe.Interfaces;
using System.Threading.Tasks;

namespace RezeptSafe.View;

public partial class QRCodeScanPopup : Popup
{

	public QRCodeScanPopup()
	{
		InitializeComponent();
		barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
		{
			Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
			AutoRotate = true,
			Multiple = false
		};
	}

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
		var first = e.Results.FirstOrDefault();

		if (first is null)
			return;

		this.Close(first.Value);
    }
}