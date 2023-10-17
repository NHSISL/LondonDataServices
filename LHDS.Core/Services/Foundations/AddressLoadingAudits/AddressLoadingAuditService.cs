using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditService : IAddressLoadingAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressLoadingAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressLoadingAudit> AddAddressLoadingAuditAsync(AddressLoadingAudit addressLoadingAudit) =>
            TryCatch(async () =>
            {
                ValidateAddressLoadingAuditOnAdd(addressLoadingAudit);

                return await this.storageBroker.InsertAddressLoadingAuditAsync(addressLoadingAudit);
            });

        public IQueryable<AddressLoadingAudit> RetrieveAllAddressLoadingAudits() =>
            TryCatch(() => this.storageBroker.SelectAllAddressLoadingAudits());

        public ValueTask<AddressLoadingAudit> RetrieveAddressLoadingAuditByIdAsync(Guid addressLoadingAuditId) =>
            TryCatch(async () =>
            {
                ValidateAddressLoadingAuditId(addressLoadingAuditId);

                AddressLoadingAudit maybeAddressLoadingAudit = await this.storageBroker
                    .SelectAddressLoadingAuditByIdAsync(addressLoadingAuditId);

                return maybeAddressLoadingAudit;
            });
    }
}