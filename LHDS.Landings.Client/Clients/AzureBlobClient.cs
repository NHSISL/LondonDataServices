// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using LHDS.Landings.Client.Brokers.Loggings;
    using Microsoft.Extensions.Azure;

    public class AzureBlobClient : IAzureBlobClient
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobServiceClient blobServiceClient;

        public AzureBlobClient(
            ILoggingBroker loggingBroker,
            BlobServiceClient defaultClient,
            IAzureClientFactory<BlobServiceClient> blobServiceFactory)
        {
            this.loggingBroker = loggingBroker;
            this.blobServiceClient = defaultClient;
        }

        public async ValueTask<MemoryStream> DownloadFileAsync(string fileName, string container)
        {
            loggingBroker.LogInformation(fileName);
            var blobClient = this.blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            return memoryStream;
        }

        public async ValueTask UploadFileAsync(string fileName, Stream stream, string container)
        {
            loggingBroker.LogInformation($"file:{fileName}, size:{stream.Length}, container:{container}");
            var blobClient = this.blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            var streamLenght = stream.Length;

            var options = new BlobUploadOptions
            {
                ProgressHandler = new Progress<long>(progress =>
                {
                    Console.WriteLine(
                        $"file: {fileName}, progress: {progress}/{streamLenght}, " +
                        $"percent:{Math.Round((double)progress / (double)streamLenght * 100.0, 2)}");
                }),
                TransferOptions = new Azure.Storage.StorageTransferOptions()
                {
                    InitialTransferSize = stream.Length
                }
            };

            await blobClient.UploadAsync(stream, options);
        }
        
        public async ValueTask DeleteFileAsync(string fileName, string container)
        {
            loggingBroker.LogInformation(fileName);
            var blobClient = this.blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
        }
    }
}