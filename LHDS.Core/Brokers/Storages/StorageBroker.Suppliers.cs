using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using LHDS.Core.Models.Suppliers;

namespace LHDS.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Supplier> Suppliers { get; set; }

        public async ValueTask<Supplier> InsertSupplierAsync(Supplier supplier)
        {
            using var broker =
                new StorageBroker(this.configuration);

            EntityEntry<Supplier> supplierEntityEntry =
                await broker.Suppliers.AddAsync(supplier);

            await broker.SaveChangesAsync();

            return supplierEntityEntry.Entity;
        }

        public IQueryable<Supplier> SelectAllSuppliers()
        {
            using var broker =
                new StorageBroker(this.configuration);

            return broker.Suppliers;
        }
    }
}
