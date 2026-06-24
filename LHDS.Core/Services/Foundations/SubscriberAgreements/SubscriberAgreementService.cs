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
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService : ISubscriberAgreementService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberAgreementService(
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

        public ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                SubscriberAgreement subscriberAgreementWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(subscriberAgreement);

                await ValidateSubscriberAgreementOnAddAsync(subscriberAgreementWithAddAuditApplied);

                return await this.storageBroker.InsertSubscriberAgreementAsync(subscriberAgreement);
            });

        public ValueTask<IQueryable<SubscriberAgreement>> RetrieveAllSubscriberAgreementsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllSubscriberAgreementsAsync());

        public ValueTask<SubscriberAgreement> RetrieveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementId(subscriberAgreementId);

                SubscriberAgreement maybeSubscriberAgreement = await this.storageBroker
                    .SelectSubscriberAgreementByIdAsync(subscriberAgreementId);

                ValidateStorageSubscriberAgreement(maybeSubscriberAgreement, subscriberAgreementId);

                return maybeSubscriberAgreement;
            });

        public ValueTask<SubscriberAgreement> ModifySubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                SubscriberAgreement subscriberAgreementWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(subscriberAgreement);

                await ValidateSubscriberAgreementOnModifyAsync(subscriberAgreementWithModifyAuditApplied);

                SubscriberAgreement maybeSubscriberAgreement =
                    await this.storageBroker.SelectSubscriberAgreementByIdAsync(
                        subscriberAgreement.Id);

                ValidateStorageSubscriberAgreement(maybeSubscriberAgreement, subscriberAgreement.Id);

                SubscriberAgreement SubscriberAgreementWithModifyAuditAppliedEnsured =
                   await this.securityAuditBroker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                       subscriberAgreement,
                       maybeSubscriberAgreement);

                ValidateAgainstStorageSubscriberAgreementOnModify(
                    inputSubscriberAgreement: SubscriberAgreementWithModifyAuditAppliedEnsured,
                    storageSubscriberAgreement: maybeSubscriberAgreement);

                return await this.storageBroker.UpdateSubscriberAgreementAsync(subscriberAgreement);
            });

        public ValueTask<SubscriberAgreement> RemoveSubscriberAgreementByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementId(optOutId);

                SubscriberAgreement maybeSubscriberAgreement = await this.storageBroker
                    .SelectSubscriberAgreementByIdAsync(optOutId);

                ValidateStorageSubscriberAgreement(maybeSubscriberAgreement, optOutId);

                return await this.storageBroker.DeleteSubscriberAgreementAsync(maybeSubscriberAgreement);
            });
    }
}