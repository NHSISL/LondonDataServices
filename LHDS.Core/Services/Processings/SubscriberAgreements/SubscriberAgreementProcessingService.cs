// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingService : ISubscriberAgreementProcessingService
    {
        private readonly ISubscriberAgreementService subscriberAgreementService;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberAgreementProcessingService(
            ISubscriberAgreementService subscriberAgreementService,
            ILoggingBroker loggingBroker)
        {
            this.subscriberAgreementService = subscriberAgreementService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            throw new NotImplementedException();

        public IQueryable<SubscriberAgreement> RetrieveAllSubscriberAgreements() =>
            throw new NotImplementedException();

        public ValueTask<SubscriberAgreement> RetrieveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberAgreement> ModifySubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            throw new NotImplementedException();

        public ValueTask<SubscriberAgreement> RemoveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            throw new NotImplementedException();
    }
}