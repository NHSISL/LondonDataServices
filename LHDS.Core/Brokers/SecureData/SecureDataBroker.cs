// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Models.Brokers.SecureData;

namespace LHDS.Core.Brokers.KeyVaults
{
    public class SecureDataBroker : ISecureDataBroker
    {
        private readonly SecretClient secretClient;

        public SecureDataBroker(string keyVaultUri)
        {
            secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        public async ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(SecureData keyVaultSecret) =>
            await this.secretClient.SetSecretAsync(keyVaultSecret.Name, keyVaultSecret.Value);

        public async ValueTask<KeyVaultSecret> GetKeyVaultSecretAsync(string secretName) =>
            await this.secretClient.GetSecretAsync(secretName);

        public async ValueTask DeleteKeyVaultSecretAsync(string secretName) =>
            await this.secretClient.StartDeleteSecretAsync(secretName);
    }
}
