using CommunityToolkit.Maui.Views;
using RezeptSafe.Interfaces;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace RezeptSafe.View;

public partial class QRCodeScanner : ContentPage
{
    private IRezeptShareService shareService;

    public QRCodeScanner(IRezeptShareService shareService)
    {
        InitializeComponent();

        this.shareService = shareService;

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }

    private bool _alreadyClosed = false;

    private async void barcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();
        if (first is not null)
        {
            this.shareService.CompleteScan(first.Value);
            await Shell.Current.GoToAsync("..");
        }
    }
}
