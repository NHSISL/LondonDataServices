// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Supplier> Suppliers { get; set; }

        public async ValueTask<Supplier> InsertSupplierAsync(
            Supplier supplier,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(supplier, cancellationToken);

        public async ValueTask<IQueryable<Supplier>> SelectAllSuppliersAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<Supplier>(cancellationToken);

        public async ValueTask<Supplier> SelectSupplierByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<Supplier>(new object[] { id }, cancellationToken);

        public async ValueTask<Supplier> UpdateSupplierAsync(
            Supplier supplier,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(supplier, cancellationToken);

        public async ValueTask<Supplier> DeleteSupplierAsync(
            Supplier supplier,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(supplier, cancellationToken);
    }
}
