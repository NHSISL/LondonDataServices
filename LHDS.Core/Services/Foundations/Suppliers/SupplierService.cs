using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages;
using LHDS.Core.Models.Suppliers;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService : ISupplierService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public SupplierService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Supplier> AddSupplierAsync(Supplier supplier) =>
            TryCatch(async () =>
            {
                ValidateSupplierOnAdd(supplier);

                return await this.storageBroker.InsertSupplierAsync(supplier);
            });

        public IQueryable<Supplier> RetrieveAllSuppliers() =>
            TryCatch(() => this.storageBroker.SelectAllSuppliers());

        public ValueTask<Supplier> RetrieveSupplierByIdAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateSupplierId(supplierId);

                Supplier maybeSupplier = await this.storageBroker
                    .SelectSupplierByIdAsync(supplierId);

                ValidateStorageSupplier(maybeSupplier, supplierId);

                return maybeSupplier;
            });

        public ValueTask<Supplier> ModifySupplierAsync(Supplier supplier) =>
            throw new NotImplementedException();
    }
}