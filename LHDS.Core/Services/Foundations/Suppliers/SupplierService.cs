// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                ValidateSupplierOnAdd(supplier);

                return await this.storageBroker.InsertSupplierAsync(supplier);
            });

        public IQueryable<Supplier> RetrieveAllSuppliers() =>
            TryCatch(() => this.storageBroker.SelectAllSuppliers());
    }
}