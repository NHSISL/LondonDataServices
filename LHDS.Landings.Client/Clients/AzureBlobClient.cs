// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace NEL.DDS.InterfaceLayer.Function.Download.Client.AzureBlobs
{
    using System;
    using System.Collections.Generic;
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
        private readonly BlobServiceClient copyToBlobServiceClient;

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

        public async ValueTask CopyFileToAlternativeStorageAsync(
            string filename,
            string destinationRoot,
            Stream stream,
            string destinationContainer)
        {
            loggingBroker.LogInformation($"Copying file: {filename}");

            var destinationBlobClient = this.copyToBlobServiceClient
                .GetBlobContainerClient(destinationContainer)
                    .GetBlobClient($"{destinationRoot}/{filename}");

            await destinationBlobClient.UploadAsync(stream, overwrite: true);
        }

        public async ValueTask CopyFileAsync(
            string fileName,
            string sourceContainer,
            string destinationContainer)
        {
            loggingBroker.LogInformation(fileName);

            var sourceBlobClient = this.blobServiceClient
                .GetBlobContainerClient(sourceContainer).GetBlobClient(fileName);

            var destinationBlobClient = this.blobServiceClient
                .GetBlobContainerClient(destinationContainer).GetBlobClient(fileName);

            var copy = await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
            await copy.WaitForCompletionAsync();
        }

        public async ValueTask<List<string>> SearchFileNamesAsync(string prefix, string container)
        {
            var sourceBlobClient = this.blobServiceClient.GetBlobContainerClient(container);
            var resultSegment = sourceBlobClient.GetBlobsByHierarchyAsync(prefix: prefix).AsPages(default, 100);
            List<string> matchedFiles = new List<string>();

            await foreach (Azure.Page<BlobHierarchyItem> blobPage in resultSegment)
            {
                foreach (BlobHierarchyItem blobhierarchyItem in blobPage.Values)
                {
                    if (blobhierarchyItem.IsPrefix)
                    {
                        // Write out the prefix of the virtual directory.
                        Console.WriteLine("Virtual directory prefix: {0}", blobhierarchyItem.Prefix);
                    }
                    else
                    {
                        // Write out the name of the blob.
                        Console.WriteLine("Blob name: {0}", blobhierarchyItem.Blob.Name);

                        matchedFiles.Add(blobhierarchyItem.Blob.Name);
                    }
                }
            }

            return matchedFiles;
        }
        public async ValueTask DeleteFileAsync(string fileName, string container)
        {
            loggingBroker.LogInformation(fileName);
            var blobClient = this.blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
        }
    }
}