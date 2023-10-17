using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditService : IAddressExtractionAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressExtractionAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressExtractionAudit> AddAddressExtractionAuditAsync(AddressExtractionAudit addressExtractionAudit) =>
            throw new System.NotImplementedException();
    }
}