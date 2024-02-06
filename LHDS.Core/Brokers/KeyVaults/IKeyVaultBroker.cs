// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.KeyVaults;

namespace LHDS.Core.Brokers.KeyVaults
{
    public interface IKeyVaultBroker
    {
        ValueTask<KeyVaultSecret> CreateOrUpdateKeyVaultSecretAsync(KeyVaultSecret keyVaultSecret);
    }
}
