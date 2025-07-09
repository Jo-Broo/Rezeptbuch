using Rezeptbuch.Interfaces;
using System.IO.Compression;
using System.Text;

namespace Rezeptbuch.Services
{
    public class RezeptShareService : IRezeptShareService
    {
        private TaskCompletionSource<string?>? _scanCompletionSource;

        public string CompressJsonToBase64(string json)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            using var outputStream = new MemoryStream();
            using (var gzip = new GZipStream(outputStream, CompressionLevel.Optimal))
            {
                gzip.Write(jsonBytes, 0, jsonBytes.Length);
            }

            byte[] compressedBytes = outputStream.ToArray();
            return Convert.ToBase64String(compressedBytes);
        }

        public string DecompressBase64ToJson(string base64)
        {
            byte[] compressedBytes = Convert.FromBase64String(base64);

            using var inputStream = new MemoryStream(compressedBytes);
            using var gzip = new GZipStream(inputStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();

            gzip.CopyTo(outputStream);
            byte[] decompressedBytes = outputStream.ToArray();

            return Encoding.UTF8.GetString(decompressedBytes);
        }

        public Task<string?> WaitForScanAsync()
        {
            _scanCompletionSource = new TaskCompletionSource<string?>();
            return _scanCompletionSource.Task;
        }

        public void CompleteScan(string? result)
        {
            _scanCompletionSource?.TrySetResult(result);
        }
    }
}
