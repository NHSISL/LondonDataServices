// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.SubscriberPractices;

namespace LHDS.Core.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeService : ISubscriberPracticeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberPracticeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SubscriberPractice> AddSubscriberPracticeAsync(SubscriberPractice subscriberPractice) =>
            TryCatch(async () =>
            {
                SubscriberPractice subscriberPracticeWithAddAuditApplied =
                    await ApplyAddAuditAsync(subscriberPractice);

                await ValidateSubscriberPracticeOnAddAsync(subscriberPracticeWithAddAuditApplied);

                return await this.storageBroker.InsertSubscriberPracticeAsync(subscriberPractice);
            });

        public ValueTask<IQueryable<SubscriberPractice>> RetrieveAllSubscriberPracticesAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllSubscriberPracticesAsync());

        public ValueTask<SubscriberPractice> RetrieveSubscriberPracticeByIdAsync(Guid subscriberPracticeId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberPracticeId(subscriberPracticeId);

                SubscriberPractice maybeSubscriberPractice = await this.storageBroker
                    .SelectSubscriberPracticeByIdAsync(subscriberPracticeId);

                ValidateStorageSubscriberPractice(maybeSubscriberPractice, subscriberPracticeId);

                return maybeSubscriberPractice;
            });

        public ValueTask<SubscriberPractice> ModifySubscriberPracticeAsync(SubscriberPractice subscriberPractice) =>
            TryCatch(async () =>
            {
                SubscriberPractice osubscriberPracticeWithModifyAuditApplied =
                    await ApplyModifyAuditAsync(subscriberPractice);

                await ValidateSubscriberPracticeOnModifyAsync(osubscriberPracticeWithModifyAuditApplied);

                SubscriberPractice maybeSubscriberPractice =
                    await this.storageBroker.SelectSubscriberPracticeByIdAsync(
                        subscriberPractice.Id);

                ValidateStorageSubscriberPractice(maybeSubscriberPractice, subscriberPractice.Id);

                SubscriberPractice SubscriberPracticeWithModifyAuditAppliedEnsured =
                   await EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
                       subscriberPractice,
                       maybeSubscriberPractice);

                ValidateAgainstStorageSubscriberPracticeOnModify(
                    inputSubscriberPractice: SubscriberPracticeWithModifyAuditAppliedEnsured,
                    storageSubscriberPractice: maybeSubscriberPractice);

                return await this.storageBroker.UpdateSubscriberPracticeAsync(subscriberPractice);
            });

        public ValueTask<SubscriberPractice> RemoveSubscriberPracticeByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberPracticeId(optOutId);

                SubscriberPractice maybeSubscriberPractice = await this.storageBroker
                    .SelectSubscriberPracticeByIdAsync(optOutId);

                ValidateStorageSubscriberPractice(maybeSubscriberPractice, optOutId);

                return await this.storageBroker.DeleteSubscriberPracticeAsync(maybeSubscriberPractice);
            });

        virtual internal async ValueTask<SubscriberPractice> ApplyAddAuditAsync(SubscriberPractice subscriberPractice)
        {
            ValidateSubscriberPracticeIsNotNull(subscriberPractice);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            subscriberPractice.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            subscriberPractice.CreatedDate = auditDateTimeOffset;
            subscriberPractice.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            subscriberPractice.UpdatedDate = auditDateTimeOffset;

            return subscriberPractice;
        }

        virtual internal async ValueTask<SubscriberPractice> ApplyModifyAuditAsync(SubscriberPractice subscriberPractice)
        {
            ValidateSubscriberPracticeIsNotNull(subscriberPractice);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            subscriberPractice.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            subscriberPractice.UpdatedDate = auditDateTimeOffset;

            return subscriberPractice;
        }

        virtual internal async ValueTask<SubscriberPractice> EnsureCreatedAuditPropertiesIsSameAsStorageAsync(
            SubscriberPractice subscriberPractice,
            SubscriberPractice maybeSubscriberPractice)
        {
            subscriberPractice.CreatedDate = maybeSubscriberPractice.CreatedDate;
            subscriberPractice.CreatedBy = maybeSubscriberPractice.CreatedBy;

            return subscriberPractice;
        }
    }
}