// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SubscriberAgreement> InsertSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        IQueryable<SubscriberAgreement> SelectAllSubscriberAgreements();
        ValueTask<SubscriberAgreement> SelectSubscriberAgreementByIdAsync(Guid subscriberAgreementId);
        ValueTask<SubscriberAgreement> UpdateSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<SubscriberAgreement> DeleteSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement);
        ValueTask<SubscriberAgreement> SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(
           Guid SupplierSharingAgreementGuid);
    }
}