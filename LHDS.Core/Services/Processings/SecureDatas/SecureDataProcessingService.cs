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

        public async ValueTask<SubscriberCredential> AddOrModifySecureDataAsync(SubscriberCredential subscriberCredential)
        {
            List<SecureData> secureData = GetSecureDataItems();  

        foreach (var data in secureData)  
        {  
            await this.secureDataService.AddOrModifySecureData(data);  
        }

            SubscriberCredential returnedSubscriberCredential = new SubscriberCredential
            {
                Id = subscriberCredential.Id,
                SupplierSharingAgreementShortName = subscriberCredential.SupplierSharingAgreementShortName,
                SupplierSharingAgreementGuid = subscriberCredential.SupplierSharingAgreementGuid,
                FtpUserName = subscriberCredential.FtpUserName,
                FtpPublicKey = subscriberCredential.FtpPublicKey,
                GpgPublicKey = subscriberCredential.GpgPublicKey,
                IsActive = subscriberCredential.IsActive,
                LastPollEndDate = subscriberCredential.LastPollEndDate,
                LastPollStartDate = subscriberCredential.LastPollStartDate
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

                SecureData returnedSecureData =  await this.secureDataService.AddOrModifySecureData(secureData);

                SetDynamicPropertyValue(returnedSubscriberCredential, keyType, returnedSecureData.Value);
            }

            return returnedSubscriberCredential;
        }

        public ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(
            string subscriberAgreementName) =>
                throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSecureData(SubscriberCredential subscriberCredential) =>
            throw new NotImplementedException();

        private static string GetPropertyValue(SubscriberCredential subscriberCredential, string propertyName)
        {
            PropertyInfo propertyInfo = typeof(SubscriberCredential).GetProperty(propName);
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
