// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SubscriberAgreement> SubscriberAgreements { get; set; }

        public async ValueTask<SubscriberAgreement> InsertSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(subscriberAgreement, cancellationToken);

        public async ValueTask<IQueryable<SubscriberAgreement>> SelectAllSubscriberAgreementsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<SubscriberAgreement>(cancellationToken);

        public async ValueTask<SubscriberAgreement> SelectSubscriberAgreementByIdAsync(
            Guid subscriberAgreementId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<SubscriberAgreement>(new object[] { subscriberAgreementId }, cancellationToken);

        public async ValueTask<SubscriberAgreement> UpdateSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(subscriberAgreement, cancellationToken);

        public async ValueTask<SubscriberAgreement> DeleteSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(subscriberAgreement, cancellationToken);
    }
}
