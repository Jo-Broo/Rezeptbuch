using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeptSafe.Interfaces
{
    public interface IRezeptShareService
    {
        string CompressJsonToBase64(string json);
        string DecompressBase64ToJson(string base64);
        Task<string?> WaitForScanAsync();
        void CompleteScan(string? result);
    }
}
