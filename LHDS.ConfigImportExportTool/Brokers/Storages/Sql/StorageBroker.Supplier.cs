// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Supplier> Suppliers { get; set; }

        public async ValueTask<Supplier> InsertSupplierAsync(Supplier supplier) =>
            await InsertAsync(supplier);

        public async ValueTask<IQueryable<Supplier>> SelectAllSuppliersAsync() =>
            await SelectAllAsync<Supplier>();

        public async ValueTask<Supplier> SelectSupplierByIdAsync(Guid id) =>
            await SelectAsync<Supplier>(id);

        public async ValueTask<Supplier> UpdateSupplierAsync(Supplier supplier) =>
            await UpdateAsync(supplier);

        public async ValueTask<Supplier> DeleteSupplierAsync(Supplier supplier) =>
            await DeleteAsync(supplier);
    }
}
