// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;

namespace LHDS.Core.Brokers.KeyVaults
{
    public interface ISecureDataBroker
    {
        ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(KeyVaultSecret keyVaultSecret);
        ValueTask<KeyVaultSecret> GetKeyVaultSecretAsync(string secretName);
        ValueTask DeleteKeyVaultSecretAsync(string secretName);
    }
}
