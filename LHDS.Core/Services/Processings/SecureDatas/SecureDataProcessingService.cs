// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Foundations.SecureDatas;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingService : ISecureDataProcessingService
    {
        private readonly ISecureDataService secureDataService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IIdentifierBroker identifierBroker;

        public SecureDataProcessingService(
            ISecureDataService secureDataService,
            ILoggingBroker loggingBroker,
            IIdentifierBroker identifierBroker)
        {
            this.secureDataService = secureDataService;
            this.loggingBroker = loggingBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<SubscriberCredential> AddOrModifySecureDataAsync(
            SubscriberCredential subscriberCredential) =>
            TryCatch(async () =>
            {
                ValidateSubscriberCredentialOnAdd(subscriberCredential);
                List<SecureData> secureData = GetSecureDataItems(subscriberCredential);

                foreach (var data in secureData)
                {
                    await this.secureDataService.AddOrModifySecureData(data);
                }

                return subscriberCredential;
            });

        public ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(
            string subscriberAgreementName) =>
                throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSecureData(SubscriberCredential subscriberCredential) =>
            throw new NotImplementedException();

        private static List<SecureData> GetSecureDataItems(SubscriberCredential subscriberCredential)
        {
            List<string> keyTypes = new List<string>
            {
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey",
            };

            List<SecureData> secureDataList = new List<SecureData>();

            foreach (string keyType in keyTypes)
            {

                string secretName = $"{subscriberCredential.Id}-{keyType}";
                string secretValue = GetPropertyValue(subscriberCredential, keyType);

                SecureData secureData = new SecureData
                {
                    Name = secretName,
                    Value = secretValue
                };

                secureDataList.Add(secureData);
            }

            return secureDataList;
        }

        private static string GetPropertyValue(SubscriberCredential subscriberCredential, string propertyName)
        {
            PropertyInfo propertyInfo = typeof(SubscriberCredential).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(subscriberCredential);
                return value?.ToString() ?? string.Empty;
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found on object.");
            }
        }
    }
}
