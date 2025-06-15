using RezeptSafe.Interfaces;
using System.IO.Compression;
using System.Text;

namespace RezeptSafe.Services
{
    internal class RezeptShareService : IRezeptShareService
    {
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
    }
}
