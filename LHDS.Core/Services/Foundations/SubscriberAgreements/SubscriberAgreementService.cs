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
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberAgreementService(
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

        public ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                await ValidateSubscriberAgreementOnAddAsync(subscriberAgreement);

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
                await ValidateSubscriberAgreementOnModifyAsync(subscriberAgreement);

                SubscriberAgreement maybeSubscriberAgreement =
                    await this.storageBroker.SelectSubscriberAgreementByIdAsync(subscriberAgreement.Id);

                ValidateStorageSubscriberAgreement(maybeSubscriberAgreement, subscriberAgreement.Id);

                ValidateAgainstStorageSubscriberAgreementOnModify(
                    inputSubscriberAgreement: subscriberAgreement,
                    storageSubscriberAgreement: maybeSubscriberAgreement);

                return await this.storageBroker.UpdateSubscriberAgreementAsync(subscriberAgreement);
            });

        public ValueTask<SubscriberAgreement> RemoveSubscriberAgreementByIdAsync(Guid subscriberAgreementId) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementId(subscriberAgreementId);

                SubscriberAgreement maybeSubscriberAgreement = await this.storageBroker
                    .SelectSubscriberAgreementByIdAsync(subscriberAgreementId);

                ValidateStorageSubscriberAgreement(maybeSubscriberAgreement, subscriberAgreementId);

                return await this.storageBroker.DeleteSubscriberAgreementAsync(maybeSubscriberAgreement);
            });
    }
}