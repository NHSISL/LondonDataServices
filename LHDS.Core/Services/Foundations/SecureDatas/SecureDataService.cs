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
        private readonly IKeyVaultSecretBroker keyVaultSecretBroker;
        private readonly ILoggingBroker loggingBroker;

        public SecureDataService(
            IKeyVaultSecretBroker keyVaultSecretBroker,
            ILoggingBroker loggingBroker)
        {
            this.keyVaultSecretBroker = keyVaultSecretBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SecureData> AddOrModifySecureData(SecureData secureData) =>
            TryCatch(async () =>
            {
                ValidateSecureDataOnAdd(secureData);

                string value = string.IsNullOrWhiteSpace(secureData.Value) ? "null" : secureData.Value;
                KeyVaultSecret keyVaultSecret = new KeyVaultSecret(name: secureData.Name, value);

                KeyVaultSecret returnedKeyVaultSecret =
                    await this.keyVaultSecretBroker.CreateOrUpdateKeyVaultSecretAsync(keyVaultSecret);

                SecureData returnedSecureData = new SecureData
                {
                    Name = returnedKeyVaultSecret.Name,
                    Value = returnedKeyVaultSecret.Value == "null" ? null : returnedKeyVaultSecret.Value,
                };

                return returnedSecureData;
            });

        public ValueTask<SecureData> RetrieveSecretDataByNameAsync(string secretName) =>
            TryCatch(async () =>
            {
                ValidateArgumentOnRetrieve(secretName);

                KeyVaultSecret returnedKeyVaultSecret =
                    await this.keyVaultSecretBroker.GetKeyVaultSecretAsync(secretName);

                SecureData returnedSecureData = new SecureData
                {
                    Name = returnedKeyVaultSecret.Name,
                    Value = returnedKeyVaultSecret.Value == "null" ? null : returnedKeyVaultSecret.Value,
                };

                return returnedSecureData;
            });

        public ValueTask RemoveSecureDataAsync(string secretName) =>
            TryCatch(async () =>
            {
                ValidateArgumentOnRemove(secretName);
                await this.keyVaultSecretBroker.DeleteKeyVaultSecretAsync(secretName);
            });
    }
}