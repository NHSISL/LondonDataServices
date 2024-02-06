// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure.Identity;
using AzureSecret = Azure.Security.KeyVault.Secrets;
using LHDSecret = LHDS.Core.Models.Brokers.KeyVaults;

namespace LHDS.Core.Brokers.KeyVaults
{
    public class KeyVaultBroker : IKeyVaultBroker
    {
        private readonly AzureSecret.SecretClient secretClient;

        public KeyVaultBroker(string keyVaultUri)
        {
            secretClient = new AzureSecret.SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        public async ValueTask<LHDSecret.KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(
            LHDSecret.KeyVaultSecret keyVaultSecret)
        {
            AzureSecret.KeyVaultSecret azureSecret = 
                await secretClient.SetSecretAsync(keyVaultSecret.Name, keyVaultSecret.Value);

            LHDSecret.KeyVaultSecret response = new LHDSecret.KeyVaultSecret
            {
                Name = azureSecret.Name,
                Value = azureSecret.Value
            };

            return response;
        }
    }
}
