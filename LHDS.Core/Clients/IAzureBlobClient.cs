// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Decryptions.Client.Clients
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IAzureBlobClient
    {
        ValueTask UploadFileAsync(string fileName, Stream stream, string container);
        ValueTask<MemoryStream> DownloadFileAsync(string fileName, string container);
        ValueTask DeleteFileAsync(string fileName, string container);
    }
}