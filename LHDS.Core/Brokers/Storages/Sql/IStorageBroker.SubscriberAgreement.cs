// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<SubscriberAgreement> InsertSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<SubscriberAgreement>> SelectAllSubscriberAgreementsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberAgreement> SelectSubscriberAgreementByIdAsync(
            Guid subscriberAgreementId,
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberAgreement> UpdateSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default);

        ValueTask<SubscriberAgreement> DeleteSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default);
    }
}