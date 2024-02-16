// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
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
                var exceptions = new List<Exception>();

                foreach (var data in secureData)
                {
                    try
                    {
                        ValidateSecureData(data);

                        await TryCatch(async () =>
                        {
                            await this.secureDataService.AddOrModifySecureData(secureData: data);
                        });
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to add or modify {exceptions.Count} secure data",
                        exceptions);
                }

                return subscriberCredential;
            });

        public ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(
            string subscriberAgreementName) =>
                throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSecureDataAsync(SubscriberCredential subscriberCredential) =>
            throw new NotImplementedException();

        private static List<SecureData> GetSecureDataItems(SubscriberCredential subscriberCredential)
        {
            List<string> keyTypes = new List<string>
            {
                "FtpPassword",
                "FtpPassPhrase",
                "FtpPrivateKey",
                "GpgPassPhrase",
                "GpgPrivateKey",
            };

            List<SecureData> secureDataList = new List<SecureData>();

            foreach (string keyType in keyTypes)
            {

                string secretName = $"{subscriberCredential.Id}-{keyType}";

                string secretValue = GetPropertyValue(
                    subscriberCredential: subscriberCredential, 
                    propertyName: keyType);

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
                throw new InvalidArgumentSubscriberCredentialProcessingException(
                    message: $"Property '{propertyName}' not found on object.");
            }
        }
    }
}
