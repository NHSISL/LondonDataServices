using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService : IAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddressOnAdd(address);

                return await this.storageBroker.InsertAddressAsync(address);
            });

        public IQueryable<Address> RetrieveAllAddresses() =>
            TryCatch(() => this.storageBroker.SelectAllAddresses());

        public ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId) =>
            TryCatch(async () =>
            {
                ValidateAddressId(addressId);

                Address maybeAddress = await this.storageBroker
                    .SelectAddressByIdAsync(addressId);

                ValidateStorageAddress(maybeAddress, addressId);

                return maybeAddress;
            });

        public ValueTask<Address> ModifyAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddressOnModify(address);

                Address maybeAddress =
                    await this.storageBroker.SelectAddressByIdAsync(address.Id);

                ValidateStorageAddress(maybeAddress, address.Id);
                ValidateAgainstStorageAddressOnModify(inputAddress: address, storageAddress: maybeAddress);

                return await this.storageBroker.UpdateAddressAsync(address);
            });

        public ValueTask<Address> RemoveAddressByIdAsync(Guid addressId) =>
            TryCatch(async () =>
            {
                ValidateAddressId(addressId);

                Address maybeAddress = await this.storageBroker
                    .SelectAddressByIdAsync(addressId);

                ValidateStorageAddress(maybeAddress, addressId);

                return await this.storageBroker.DeleteAddressAsync(maybeAddress);
            });

        public List<Address> RetrieveAddressesByPostCode(string postCode) =>
            throw new System.NotImplementedException();
    }
}