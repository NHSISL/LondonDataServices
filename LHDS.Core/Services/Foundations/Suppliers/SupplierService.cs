// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Suppliers;

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
                await ValidateSupplierOnAddAsync(supplier);

                Supplier maybeSupplier =
                   await this.storageBroker.SelectSupplierByIdAsync(supplier.Id);

                if (maybeSupplier is null)
                {
                    return await this.storageBroker.InsertSupplierAsync(supplier);
                }

                return maybeSupplier;
            });

        public ValueTask<IQueryable<Supplier>> RetrieveAllSuppliersAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllSuppliersAsync());

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
            TryCatch(async () =>
            {
                await ValidateSupplierOnModifyAsync(supplier);

                Supplier maybeSupplier =
                    await this.storageBroker.SelectSupplierByIdAsync(supplier.Id);

                ValidateStorageSupplier(maybeSupplier, supplier.Id);
                ValidateAgainstStorageSupplierOnModify(inputSupplier: supplier, storageSupplier: maybeSupplier);

                return await this.storageBroker.UpdateSupplierAsync(supplier);
            });

        public ValueTask<Supplier> RemoveSupplierByIdAsync(Guid supplierId) =>
            TryCatch(async () =>
            {
                ValidateSupplierId(supplierId);

                Supplier maybeSupplier = await this.storageBroker
                    .SelectSupplierByIdAsync(supplierId);

                ValidateStorageSupplier(maybeSupplier, supplierId);

                return await this.storageBroker.DeleteSupplierAsync(maybeSupplier);
            });
    }
}