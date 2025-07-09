namespace Rezeptbuch.Interfaces
{
    public interface IRezeptShareService
    {
        string CompressJsonToBase64(string json);
        string DecompressBase64ToJson(string base64);
        Task<string?> WaitForScanAsync();
        void CompleteScan(string? result);
    }
}
