// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs;

namespace LHDS.Landings.Client.Brokers
{
    public class BlobStorageBroker : IBlobStorageBroker
    {
        private readonly IAzureBlobClient azureBlobClient;
        private readonly IConfiguration configuration;

        public BlobStorageBroker(IAzureBlobClient azureBlobClient, IConfiguration configuration)
        {
            this.azureBlobClient = azureBlobClient;
            this.configuration = configuration;
        }

        public async ValueTask InsertFileAsync(string fileName, Stream stream, string container) =>
            await azureBlobClient.UploadFileAsync(fileName, stream, container);

        public async ValueTask<string> SelectDownloadFileLinkByFileNameAsync(string fileName, string container)
        {
            MemoryStream ms = await azureBlobClient.DownloadFileAsync(fileName, container);

            return ms.ToString();
        }
        public async ValueTask DeleteFileAsync(string fileName, string container) =>
            await azureBlobClient.DeleteFileAsync(fileName, container);
    }
}