// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Clients;

namespace LHDS.Core.Brokers.Storages.Blobs
{
    public class BlobStorageBroker : IBlobStorageBroker
    {
        private readonly IAzureBlobClient azureBlobClient;

        public BlobStorageBroker(IAzureBlobClient azureBlobClient)
        {
            this.azureBlobClient = azureBlobClient;
        }

        public async ValueTask InsertFileAsync(string fileName, Stream stream, string container) =>
            await azureBlobClient.UploadFileAsync(
                fileName,
                stream,
                container);

        public async ValueTask<byte[]> SelectByFileNameAsync(string fileName, string container)
        {
            MemoryStream ms = await azureBlobClient
                .DownloadFileAsync(fileName, container);

            return ms.ToArray();
        }

        public async ValueTask DeleteFileAsync(string fileName, string container) =>
            await azureBlobClient.DeleteFileAsync(fileName, container);

        public async ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn)
        {
            Uri uri = await azureBlobClient.GetDownloadUriAsync(
                fileName,
                container,
                expiresOn);

            return uri.ToString();
        }
    }
}