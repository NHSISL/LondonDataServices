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
using LHDS.Core.Models.Foundations.Suppliers;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService : ISupplierService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public SupplierService(
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

        public ValueTask<Supplier> AddSupplierAsync(Supplier supplier) =>
            TryCatch(async () =>
            {
                Supplier supplierWithAddAuditApplied = await ApplyAddSupplierAsync(supplier);
                await ValidateSupplierOnAddAsync(supplierWithAddAuditApplied);

                Supplier maybeSupplier =
                   await this.storageBroker.SelectSupplierByIdAsync(supplierWithAddAuditApplied.Id);

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
                Supplier supplierWithModifyAuditApplied = await ApplyModifyAuditAsync(supplier);
                await ValidateSupplierOnModifyAsync(supplierWithModifyAuditApplied);

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

        virtual internal async ValueTask<Supplier> ApplyAddSupplierAsync(Supplier supplier)
        {
            ValidateSupplierIsNotNull(supplier);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            supplier.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            supplier.CreatedDate = auditDateTimeOffset;
            supplier.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            supplier.UpdatedDate = auditDateTimeOffset;

            return supplier;
        }

        virtual internal async ValueTask<Supplier> ApplyModifyAuditAsync(Supplier supplier)
        {
            ValidateSupplierIsNotNull(supplier);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            supplier.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            supplier.UpdatedDate = auditDateTimeOffset;

            return supplier;
        }
    }
}