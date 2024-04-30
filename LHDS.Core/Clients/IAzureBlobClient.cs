// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Clients
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IAzureBlobClient
    {
        ValueTask UploadFileAsync(string fileName, Stream stream, string container);
        ValueTask<MemoryStream> DownloadFileAsync(string fileName, string container);
        ValueTask DeleteFileAsync(string fileName, string container);
        ValueTask<Uri> GetDownloadUriAsync(string fileName, string container, DateTimeOffset expiresOn);
    }
}