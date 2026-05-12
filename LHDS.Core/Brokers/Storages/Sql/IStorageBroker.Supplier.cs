// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<Supplier> InsertSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<Supplier>> SelectAllSuppliersAsync(CancellationToken cancellationToken = default);
        ValueTask<Supplier> SelectSupplierByIdAsync(Guid supplierId, CancellationToken cancellationToken = default);
        ValueTask<Supplier> UpdateSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);
        ValueTask<Supplier> DeleteSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);
    }
}
