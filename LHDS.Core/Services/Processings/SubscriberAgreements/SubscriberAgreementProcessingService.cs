// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Services.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Processings.SubscriberAgreements
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
            TryCatch(async () =>
            {
                ValidateSubscriberAgreement(subscriberAgreement);

                return await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);
            });

        public IQueryable<SubscriberAgreement> RetrieveAllSubscriberAgreements() =>
            TryCatch(() => this.subscriberAgreementService.RetrieveAllSubscriberAgreements());

        public ValueTask<SubscriberAgreement> RetrieveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementId(subscriberAgreementId);

                return await this.subscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(subscriberAgreementId);
            });

        public ValueTask<SubscriberAgreement> RetrieveOrAddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreement(subscriberAgreement);

                return await this.subscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(subscriberAgreement.Id) ??
                    await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);
            });

        public ValueTask<SubscriberAgreement> ModifyOrAddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreement(subscriberAgreement);
                ValidateSubscriberAgreementId(subscriberAgreement.Id);
                var maybeSubscriberAgreement = await this.subscriberAgreementService.RetrieveSubscriberAgreementByIdAsync(subscriberAgreement.Id);

                if (maybeSubscriberAgreement != null)
                {
                    return await this.subscriberAgreementService.ModifySubscriberAgreementAsync(subscriberAgreement);
                }
                else
                {
                    return await this.subscriberAgreementService.AddSubscriberAgreementAsync(subscriberAgreement);
                }
            });

        public ValueTask<SubscriberAgreement> ModifySubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreement(subscriberAgreement);

                return await this.subscriberAgreementService.ModifySubscriberAgreementAsync(subscriberAgreement);
            });

        public ValueTask<SubscriberAgreement> RemoveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementId(subscriberAgreementId);

                return await this.subscriberAgreementService.RemoveSubscriberAgreementByIdAsync(subscriberAgreementId);
            });
    }
}