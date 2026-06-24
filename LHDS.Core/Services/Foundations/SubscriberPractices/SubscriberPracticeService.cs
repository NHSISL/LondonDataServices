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
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberPracticeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SubscriberPractice> AddSubscriberPracticeAsync(SubscriberPractice subscriberPractice) =>
            TryCatch(async () =>
            {
                SubscriberPractice subscriberPracticeWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(subscriberPractice);

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
                SubscriberPractice subscriberPracticeWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(subscriberPractice);

                await ValidateSubscriberPracticeOnModifyAsync(subscriberPracticeWithModifyAuditApplied);

                SubscriberPractice maybeSubscriberPractice =
                    await this.storageBroker.SelectSubscriberPracticeByIdAsync(
                        subscriberPractice.Id);

                ValidateStorageSubscriberPractice(maybeSubscriberPractice, subscriberPractice.Id);

                SubscriberPractice SubscriberPracticeWithModifyAuditAppliedEnsured =
                   await this.securityAuditBroker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
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
    }
}