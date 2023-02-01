// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using LHDS.Landings.Client.Providers.Downloads.FtpDownloads;
using NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs;

namespace LHDS.Landings.Client.Brokers.Storages.Blobs
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

        public async ValueTask InsertFileAsync(string fileName, Stream stream, bool isDecrypted) =>
            await azureBlobClient.UploadFileAsync(fileName, stream, GetConatinerName(isDecrypted));

        public async ValueTask<byte[]> SelectByFileNameAsync(string fileName, bool isDecrypted)
        {
            MemoryStream ms = await azureBlobClient.DownloadFileAsync(fileName, GetConatinerName(isDecrypted));

            return ms.ToArray();
        }

        public async ValueTask DeleteFileAsync(string fileName, bool isDecrypted) =>
            await azureBlobClient.DeleteFileAsync(fileName, GetConatinerName(isDecrypted));

        private string GetConatinerName(bool isDecrypted)
        {
            var encryptedBlobContainerName = blobStorageBrokerSettings.EncryptedBlobContainerName;
            var decryptedBlobContainerName = blobStorageBrokerSettings.DecryptedBlobContainerName;

            var blobContainerName = isDecrypted
                ? decryptedBlobContainerName
                : encryptedBlobContainerName;

            return blobContainerName;
        }
    }
}