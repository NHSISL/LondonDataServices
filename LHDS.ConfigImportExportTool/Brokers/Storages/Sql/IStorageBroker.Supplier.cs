// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Suppliers;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<Supplier> InsertSupplierAsync(Supplier supplier);
        ValueTask<IQueryable<Supplier>> SelectAllSuppliersAsync();
        ValueTask<Supplier> SelectSupplierByIdAsync(Guid supplierId);
        ValueTask<Supplier> UpdateSupplierAsync(Supplier supplier);
        ValueTask<Supplier> DeleteSupplierAsync(Supplier supplier);
    }
}
