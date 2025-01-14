// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Suppliers;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public interface ISupplierService
    {
        ValueTask<Supplier> AddSupplierAsync(Supplier supplier);
        ValueTask<IQueryable<Supplier>> RetrieveAllSuppliersAsync();
        ValueTask<Supplier> RetrieveSupplierByIdAsync(Guid supplierId);
        ValueTask<Supplier> ModifySupplierAsync(Supplier supplier);
        ValueTask<Supplier> RemoveSupplierByIdAsync(Guid supplierId);
    }
}