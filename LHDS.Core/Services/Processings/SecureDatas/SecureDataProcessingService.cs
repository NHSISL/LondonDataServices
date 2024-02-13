// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
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

        public ValueTask<SubscriberCredential> AddOrModifySecureData(SubscriberCredential secureData) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RetrieveSecretsBySubscriberAgreementNameAsync(
            string subscriberAgreementName) =>
                throw new NotImplementedException();

        public ValueTask<SubscriberCredential> RemoveSecureData(SubscriberCredential secureData) =>
            throw new NotImplementedException();
    }
}