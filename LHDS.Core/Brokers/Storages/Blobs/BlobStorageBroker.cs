// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Clients;

namespace LHDS.Core.Brokers.Storages.Blobs
{
    public class BlobStorageBroker : IBlobStorageBroker
    {
        private readonly IAzureBlobClient azureBlobClient;
        private readonly IBlobStorageBrokerSettings blobStorageBrokerSettings;

        public BlobStorageBroker(IAzureBlobClient azureBlobClient, IBlobStorageBrokerSettings blobStorageBrokerSettings)
        {
            this.azureBlobClient = azureBlobClient;
            this.blobStorageBrokerSettings = blobStorageBrokerSettings;
        }

        public async ValueTask InsertFileAsync(string fileName, Stream stream) =>
            await azureBlobClient.UploadFileAsync(fileName, stream, blobStorageBrokerSettings.BlobContainerName);

        public async ValueTask<byte[]> SelectByFileNameAsync(string fileName)
        {
            MemoryStream ms = await azureBlobClient
                .DownloadFileAsync(fileName, blobStorageBrokerSettings.BlobContainerName);

            return ms.ToArray();
        }

        public async ValueTask DeleteFileAsync(string fileName) =>
            await azureBlobClient.DeleteFileAsync(fileName, blobStorageBrokerSettings.BlobContainerName);
    }
}