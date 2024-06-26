// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CryptographyKeys;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Clients;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Providers.Cryptography;
using Microsoft.Azure.Functions.Worker;

namespace LHDS.Functions.Landings.Emis
{
    public class PocEncryptDecryptionEventFunction
    {
        private readonly ILoggingBroker loggingBroker;
        private readonly IDecryptionClient decryptionClient;
        private readonly IAzureBlobClient azureBlobClient;
        private readonly ICryptographyProvider cryptographyProvider;

        public PocEncryptDecryptionEventFunction(
            ILoggingBroker loggingBroker,
            IDecryptionClient decryptionClient,
            IAzureBlobClient azureBlobClient,
            ICryptographyProvider cryptographyProvider)
        {
            this.loggingBroker = loggingBroker;
            this.decryptionClient = decryptionClient;
            this.azureBlobClient = azureBlobClient;
            this.cryptographyProvider = cryptographyProvider;
        }

        [Function("PocEncryptDecryptionEventFunction")]
        public async Task Run(
            [BlobTrigger("emislanding/poc/input/{name}", Connection = "BlobStorage")] Stream myBlob, string name)
        {
            this.loggingBroker
                .LogInformation(
                    $"C# Blob trigger function Processing blob\n " +
                    $"Name: emislanding/poc/input/{name}");

            try
            {
                var gpgKeyBroker = new GpgKeyBroker();
                var keys = await gpgKeyBroker.GenerateKeysAsync("Test");

                var subscriberCredential = new SubscriberCredential
                {
                    GpgPrivateKey = ConvertToBase64String(keys.PrivateKey),
                    GpgPublicKey = ConvertToBase64String(keys.PublicKey),
                    GpgPassPhrase = keys.Passphrase,
                };

                string tempEncryptedFilePath = Path.GetTempFileName();
                string tempDecryptedFilePath = Path.GetTempFileName();

                try
                {
                    // Encrypt the input stream directly to a file
                    using (FileStream encryptedFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await this.cryptographyProvider.EncryptAsync(
                            input: myBlob,
                            output: encryptedFileStream,
                            subscriberCredential);
                    }

                    // Upload the encrypted file to the blob storage
                    using (FileStream encryptedFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Open, FileAccess.Read))
                    {
                        await this.azureBlobClient.UploadFileAsync(
                            fileName: $"poc/encrypted/encrypted_{name}",
                            stream: encryptedFileStream,
                            container: "emislanding");
                    }

                    this.loggingBroker
                        .LogInformation(
                            $"Encrypted file:  emislanding/poc/encrypted/encrypted_{name}");

                    // Decrypt the encrypted file directly to another file
                    using (FileStream encryptedFileStream =
                        new FileStream(tempEncryptedFilePath, FileMode.Open, FileAccess.Read))

                    using (FileStream decryptedFileStream =
                        new FileStream(tempDecryptedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await this.cryptographyProvider.DecryptAsync(
                            input: encryptedFileStream,
                            output: decryptedFileStream,
                            subscriberCredential);
                    }

                    // Upload the decrypted file to the blob storage (if needed)
                    using (FileStream decryptedFileStream =
                        new FileStream(tempDecryptedFilePath, FileMode.Open, FileAccess.Read))
                    {
                        await this.azureBlobClient.UploadFileAsync(
                            fileName: $"poc/decrypted/decrypted_{name}",
                            stream: decryptedFileStream,
                            container: "emislanding");
                    }

                    this.loggingBroker
                        .LogInformation(
                            $"Encrypted file:  emislanding/poc/decrypted/decrypted_{name}");
                }
                finally
                {
                    // Clean up temporary files
                    File.Delete(tempEncryptedFilePath);
                    File.Delete(tempDecryptedFilePath);
                }
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        private static string ConvertToBase64String(string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            string base64String = Convert.ToBase64String(byteArray);

            return base64String;
        }
    }
}
