// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Processings.SubscriberAgreements
{
    public interface ISubscriberAgreementProcessingService
    {
        ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<IQueryable<SubscriberAgreement>> RetrieveAllSubscriberAgreementsAsync();
        ValueTask<SubscriberAgreement> RetrieveSubscriberAgreementByIdAsync(Guid subscriberAgreementId);
        ValueTask<SubscriberAgreement> RetrieveOrAddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<SubscriberAgreement> ModifyOrAddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<SubscriberAgreement> ModifySubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<SubscriberAgreement> RemoveSubscriberAgreementByIdAsync(Guid subscriberAgreementId);
    }
}