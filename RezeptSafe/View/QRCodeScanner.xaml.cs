using CommunityToolkit.Maui.Views;
using RezeptSafe.Interfaces;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace RezeptSafe.View;

public partial class QRCodeScanner : ContentPage
{
    private IRezeptShareService shareService;

    private bool _hasNavigatedBack;

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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (!this._hasNavigatedBack)
        {
            // Das ScanEvent hat noch keinen Barcode gelesen, die Seite wird aber bereits geschlossen
            // d.h. der Nutzer selbst hat den Vorgang abgebrochen
            this.shareService.CompleteScan(null);
        }
    }

    private async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();
        if (first is null || this._hasNavigatedBack)
        {
            return;
        }

        this._hasNavigatedBack = true;

        this.shareService.CompleteScan(first.Value);

        await MainThread.InvokeOnMainThreadAsync(async () => { await Shell.Current.Navigation.PopAsync(); });
    }
}
