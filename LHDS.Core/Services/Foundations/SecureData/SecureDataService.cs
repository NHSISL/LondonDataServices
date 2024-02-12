// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Brokers.KeyVaults;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService : ISecureDataService
    {
        private readonly ISecureDataBroker secureDataBroker;
        private readonly ILoggingBroker loggingBroker;

        public SecureDataService(
            ISecureDataBroker secureDataBroker,
            ILoggingBroker loggingBroker)
        {
            this.secureDataBroker = secureDataBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SecureData> AddOrModifySecureData(SecureData secureData) =>
            TryCatch(async () =>
            {
                ValidateSecureDataOnAdd(secureData);
                KeyVaultSecret keyVaultSecret = new KeyVaultSecret(name: secureData.Name, value: secureData.Value);

                KeyVaultSecret returnedKeyVaultSecret =
                    await this.secureDataBroker.CreateOrUpdateKeyVaultSecretAsync(keyVaultSecret);

                SecureData returnedSecureData = new SecureData
                {
                    Name = returnedKeyVaultSecret.Name,
                    Value = returnedKeyVaultSecret.Value,
                };

                return returnedSecureData;
            });

        public ValueTask<SecureData> RetrieveSecretDataByNameAsync(string secretName) =>
            TryCatch(async () =>
            {
                ValidateArgumentOnRetrieve(secretName);

                KeyVaultSecret returnedKeyVaultSecret =
                    await this.secureDataBroker.GetKeyVaultSecretAsync(secretName);

                SecureData returnedSecureData = new SecureData
                {
                    Name = returnedKeyVaultSecret.Name,
                    Value = returnedKeyVaultSecret.Value,
                };

                return returnedSecureData;
            });
    }
}