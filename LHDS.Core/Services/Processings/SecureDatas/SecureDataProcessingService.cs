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

namespace LHDS.Core.Services.Foundations.SecureDatas
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

        public ValueTask<SubscriberCredential> AddOrModifySecureData(SubscriberCredential subscriberCredential)
        {
            List<string> keyTypes = new List<string>
                {
                    "FtpPassPhrase",
                    "FtpPrivateKey",
                    "GpgPassPhrase",
                    "GpgPrivateKe",
                };

            foreach (string keyType in keyTypes)
            {
                string secretName = $"{subscriberCredential.Id}-{keyType}";
                string secretValue = GetDynamicPropertyValue(subscriberCredential, keyType);

                SecureData secureData = new SecureData
                {
                    Name = secretName,
                    Value = secretValue
                };

                this.secureDataService.AddOrModifySecureData(secureData);
            }
        }

        public ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(
            string subscriberAgreementName) =>
                throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSecureData(SubscriberCredential subscriberCredential) =>
            throw new NotImplementedException();

        private static string GetDynamicPropertyValue(dynamic obj, string propertyName)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                object value = propertyInfo.GetValue(obj);
                return value?.ToString() ?? string.Empty;
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found on object.");
            }
        }
    }
}
