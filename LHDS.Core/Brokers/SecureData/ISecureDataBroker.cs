// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Models.Foundations.SecureData;

namespace LHDS.Core.Brokers.KeyVaults
{
    public interface ISecureDataBroker
    {
        ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(SecureData secureData);
        ValueTask<KeyVaultSecret> GetKeyVaultSecretAsync(string secretName);
        ValueTask DeleteKeyVaultSecretAsync(string secretName);
    }
}
