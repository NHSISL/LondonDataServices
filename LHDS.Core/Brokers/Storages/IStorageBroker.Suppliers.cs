using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Suppliers;

namespace LHDS.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Supplier> InsertSupplierAsync(Supplier supplier);
        IQueryable<Supplier> SelectAllSuppliers();
    }
}
