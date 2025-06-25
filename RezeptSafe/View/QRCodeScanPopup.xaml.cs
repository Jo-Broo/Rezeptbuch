using CommunityToolkit.Maui.Views;
using RezeptSafe.Interfaces;
using System.Threading.Tasks;

namespace RezeptSafe.View;

public partial class QRCodeScanPopup : Popup
{
    private readonly TaskCompletionSource<string?> _tcs = new();

    public Task<string?> Result => _tcs.Task;

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

    private bool _alreadyClosed = false;

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (_alreadyClosed)
            return;

        var first = e.Results.FirstOrDefault();
        if (first is not null)
        {
            _alreadyClosed = true;
            _tcs.TrySetResult(first.Value);
            Close(); // popup schlieﬂen
        }
    }

    protected override async Task OnClosed(object? result, bool wasDismissedByTappingOutsideOfPopup, CancellationToken token = default)
    {
        await base.OnClosed(result, wasDismissedByTappingOutsideOfPopup, token);

        if (!_alreadyClosed)
            _tcs.TrySetResult(null);
    }
}
