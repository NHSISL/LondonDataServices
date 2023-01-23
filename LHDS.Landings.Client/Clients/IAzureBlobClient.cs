// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IAzureBlobClient
    {
        ValueTask UploadFileAsync(string fileName, Stream stream, string container);
        ValueTask<MemoryStream> DownloadFileAsync(string fileName, string container);
        ValueTask CopyFileAsync(string fileName, string sourceContainer, string destinationContainer);
        ValueTask<List<string>> SearchFileNamesAsync(string prefix, string container);

        ValueTask CopyFileToAlternativeStorageAsync(
            string filename,
            string destinationRoot,
            Stream stream,
            string destinationContainer);
        ValueTask DeleteFileAsync(string fileName, string container);
    }
}