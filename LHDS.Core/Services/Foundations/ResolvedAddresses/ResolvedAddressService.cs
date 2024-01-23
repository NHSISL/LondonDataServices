using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService : IResolvedAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ResolvedAddressService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressOnAdd(resolvedAddress);

                return await this.storageBroker.InsertResolvedAddressAsync(resolvedAddress);
            });

        public IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses() =>
            throw new System.NotImplementedException();
    }
}