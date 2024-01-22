using System;
using System.Linq;
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
            TryCatch(async () =>
            {
                ValidateAddressExtractionAuditOnAdd(addressExtractionAudit);

                return await this.storageBroker.InsertAddressExtractionAuditAsync(addressExtractionAudit);
            });

        public IQueryable<AddressExtractionAudit> RetrieveAllAddressExtractionAudits() =>
            TryCatch(() => this.storageBroker.SelectAllAddressExtractionAudits());

        public ValueTask<AddressExtractionAudit> RetrieveAddressExtractionAuditByIdAsync(Guid addressExtractionAuditId) =>
            TryCatch(async () =>
            {
                ValidateAddressExtractionAuditId(addressExtractionAuditId);

                AddressExtractionAudit maybeAddressExtractionAudit = await this.storageBroker
                    .SelectAddressExtractionAuditByIdAsync(addressExtractionAuditId);

                ValidateStorageAddressExtractionAudit(maybeAddressExtractionAudit, addressExtractionAuditId);

                return maybeAddressExtractionAudit;
            });

        public ValueTask<AddressExtractionAudit> ModifyAddressExtractionAuditAsync(AddressExtractionAudit addressExtractionAudit) =>
            TryCatch(async () =>
            {
                ValidateAddressExtractionAuditOnModify(addressExtractionAudit);

                AddressExtractionAudit maybeAddressExtractionAudit =
                    await this.storageBroker.SelectAddressExtractionAuditByIdAsync(addressExtractionAudit.Id);

                ValidateStorageAddressExtractionAudit(maybeAddressExtractionAudit, addressExtractionAudit.Id);
                ValidateAgainstStorageAddressExtractionAuditOnModify(inputAddressExtractionAudit: addressExtractionAudit, storageAddressExtractionAudit: maybeAddressExtractionAudit);

                return await this.storageBroker.UpdateAddressExtractionAuditAsync(addressExtractionAudit);
            });

        public ValueTask<AddressExtractionAudit> RemoveAddressExtractionAuditByIdAsync(Guid addressExtractionAuditId) =>
            TryCatch(async () =>
            {
                ValidateAddressExtractionAuditId(addressExtractionAuditId);

                AddressExtractionAudit maybeAddressExtractionAudit = await this.storageBroker
                    .SelectAddressExtractionAuditByIdAsync(addressExtractionAuditId);

                ValidateStorageAddressExtractionAudit(maybeAddressExtractionAudit, addressExtractionAuditId);

                return await this.storageBroker.DeleteAddressExtractionAuditAsync(maybeAddressExtractionAudit);
            });
    }
}