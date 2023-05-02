using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages;
using LHDS.Core.Models.PdsAudits;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService : IPdsAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PdsAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit) =>
            TryCatch(async () =>
            {
                ValidatePdsAuditOnAdd(pdsAudit);

                return await this.storageBroker.InsertPdsAuditAsync(pdsAudit);
            });

        public IQueryable<PdsAudit> RetrieveAllPdsAudits() =>
            this.storageBroker.SelectAllPdsAudits();
    }
}