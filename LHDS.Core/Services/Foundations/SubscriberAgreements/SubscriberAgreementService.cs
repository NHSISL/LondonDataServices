using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.SubscriberAgreements;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService : ISubscriberAgreementService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SubscriberAgreementService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SubscriberAgreement> AddSubscriberAgreementAsync(SubscriberAgreement subscriberAgreement) =>
            TryCatch(async () =>
            {
                ValidateSubscriberAgreementOnAdd(subscriberAgreement);

                return await this.storageBroker.InsertSubscriberAgreementAsync(subscriberAgreement);
            });

        public IQueryable<SubscriberAgreement> RetrieveAllSubscriberAgreements() =>
            TryCatch(() => this.storageBroker.SelectAllSubscriberAgreements());

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
            throw new NotImplementedException();
    }
}