// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace LHDS.Core.Clients
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Azure.Storage;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using LHDS.Core.Brokers.Hashing;
    using LHDS.Core.Brokers.Loggings;

    public class AzureBlobClient : IAzureBlobClient
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly BlobServiceClient blobServiceClient;

        public AzureBlobClient(
            ILoggingBroker loggingBroker,
            BlobServiceClient defaultClient)
        {
            this.loggingBroker = loggingBroker;
            blobServiceClient = defaultClient;
        }

        public async ValueTask DownloadFileAsync(Stream output, string fileName, string container)
        {
            await loggingBroker.LogInformationAsync(fileName);

            var blobClient = blobServiceClient
                .GetBlobContainerClient(container).GetBlobClient(fileName);

            await blobClient.DownloadToAsync(output);
        }

        public async ValueTask UploadFileAsync(Stream input, string fileName, string container)
        {
            try
            {
                if (input.CanSeek)
                    input.Position = 0;

                await using var hashingStream = new HashingCountingBroker(input, HashAlgorithmName.MD5);
                var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);

                var options = new BlobUploadOptions
                {
                    ProgressHandler = new Progress<long>(progress =>
                    {
                        Console.WriteLine($"file: {fileName}, progress: {progress}");
                    }),

                    // Upload in chunks: setting InitialTransferSize = input.Length buffers the
                    // entire file in memory and OOM-kills the process on multi-GB files.
                    TransferOptions = new StorageTransferOptions
                    {
                        MaximumTransferSize = 4 * 1024 * 1024,
                        InitialTransferSize = 4 * 1024 * 1024
                    }
                };

                await blobClient.UploadAsync(hashingStream.AsStream(), options);
                long streamLength = hashingStream.BytesRead;
                string hash = hashingStream.GetFinalHashHex();

                await loggingBroker.LogInformationAsync(
                    $"file:{fileName}, size:{streamLength}, hash:{hash}, container:{container}");
            }
            catch (Exception ex)
            {
                await loggingBroker.LogErrorAsync(ex);
                Console.WriteLine($"Unable to write blob: {fileName}");
                throw;
            }
        }

        public async ValueTask<Stream> OpenReadAsync(string fileName, string container)
        {
            await loggingBroker.LogInformationAsync(fileName);

            var blobClient = blobServiceClient
                .GetBlobContainerClient(container).GetBlobClient(fileName);

            return await blobClient.OpenReadAsync();
        }

        public async ValueTask DeleteFileAsync(string fileName, string container)
        {
            await loggingBroker.LogInformationAsync(fileName);
            var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
        }

        public async ValueTask<Uri> GetDownloadUriAsync(string fileName, string container, DateTimeOffset expiresOn)
        {
            await loggingBroker.LogInformationAsync(fileName);
            var blobClient = this.blobServiceClient.GetBlobContainerClient(container).GetBlobClient(fileName);
            var userDelegationKey = blobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b", // b for blob, c for container
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = expiresOn,
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read); // read permissions

            // Add the SAS token to the container URI.
            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, blobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri();
        }
    }
}