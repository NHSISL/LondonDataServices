// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace LHDS.Core.Brokers.KeyVaults
{
    public class KeyVaultSecretBroker : IKeyVaultSecretBroker
    {
        private readonly SecretClient secretClient;

        public IConfiguration Configuration { get; }

        public KeyVaultSecretBroker(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var keyVaultUri = this.Configuration["KeyVaultUri"];
            secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        public async ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(KeyVaultSecret keyVaultSecret) =>
            await this.secretClient.SetSecretAsync(keyVaultSecret.Name, keyVaultSecret.Value);

        public async ValueTask<KeyVaultSecret> GetKeyVaultSecretAsync(string secretName) =>
            await this.secretClient.GetSecretAsync(secretName);

        public async ValueTask DeleteKeyVaultSecretAsync(string secretName) =>
            await this.secretClient.StartDeleteSecretAsync(secretName);
    }
}
