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
            this.storageBroker.SelectAllSubscriberAgreements();
    }
}