// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;

namespace LHDS.Core.Brokers.KeyVaults
{
    public class MockKeyVaultSecretBroker : IKeyVaultSecretBroker
    {
        public async ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(KeyVaultSecret keyVaultSecret) =>
            await ValueTask.FromResult(keyVaultSecret);

        public async ValueTask<KeyVaultSecret> GetKeyVaultSecretAsync(string secretName) =>
            await ValueTask.FromResult(new KeyVaultSecret(name: secretName, value: "mock value"));

        public async ValueTask DeleteKeyVaultSecretAsync(string secretName) =>
            await ValueTask.CompletedTask;
    }
}
