// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<SubscriberAgreement> SubscriberAgreements { get; set; }

        public async ValueTask<SubscriberAgreement> InsertSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement) =>
                await InsertAsync(subscriberAgreement);

        public IQueryable<SubscriberAgreement> SelectAllSubscriberAgreements() => ReadAll<SubscriberAgreement>();

        public async ValueTask<SubscriberAgreement> SelectSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            await ReadAsync<SubscriberAgreement>(subscriberAgreementId);

        public async ValueTask<SubscriberAgreement> UpdateSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement) =>
                await UpdateAsync(subscriberAgreement);

        public async ValueTask<SubscriberAgreement> DeleteSubscriberAgreementAsync(
            SubscriberAgreement subscriberAgreement) =>
                await DeleteAsync(subscriberAgreement);

        public async ValueTask<SubscriberAgreement>
            SelectSubscriberAgreementBySupplierSharingAgreementGuidAsync(Guid SupplierSharingAgreementGuid) =>
                await ReadAsync<SubscriberAgreement>(SupplierSharingAgreementGuid);
    }
}
