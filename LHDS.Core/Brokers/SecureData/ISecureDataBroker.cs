// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Models.Brokers.SecureData;

namespace LHDS.Core.Brokers.KeyVaults
{
    public interface ISecureDataBroker
    {
        ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(SecureData secureData);
    }
}
