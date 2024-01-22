// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.Addresses;

namespace LHDS.Core.Services.Processings.Addresses
{
    public partial class AddressProcessingService : IAddressProcessingService
    {
        private readonly IAddressService addressService;
        private readonly ILoggingBroker loggingBroker;

        public AddressProcessingService(
            IAddressService addressService,
            ILoggingBroker loggingBroker)
        {
            this.addressService = addressService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddress(address);

                return await this.addressService.AddAddressAsync(address);
            });

        public IQueryable<Address> RetrieveAllAddresses() =>
            TryCatch(() => this.addressService.RetrieveAllAddresses());

        public ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId) =>
            TryCatch(async () =>
            {
                ValidateAddressId(addressId);

                return await this.addressService.RetrieveAddressByIdAsync(addressId);
            });

        public ValueTask<Address> RetrieveOrAddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddress(address);
                ValidateAddressId(address.Id);

                return await this.addressService.RetrieveAddressByIdAsync(address.Id) ??
                    await this.addressService.AddAddressAsync(address);
            });

        public ValueTask<Address> ModifyOrAddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddress(address);
                ValidateAddressId(address.Id);
                var maybeAddress = await this.addressService.RetrieveAddressByIdAsync(address.Id);

                if (maybeAddress != null)
                {
                    return await this.addressService.ModifyAddressAsync(address);
                }
                else
                {
                    return await this.addressService.AddAddressAsync(address);
                }
            });

        public ValueTask<Address> ModifyAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddress(address);

                return await this.addressService.ModifyAddressAsync(address);
            });

        public ValueTask<Address> RemoveAddressByIdAsync(Guid addressId) =>
            TryCatch(async () =>
            {
                ValidateAddressId(addressId);

                return await this.addressService.RemoveAddressByIdAsync(addressId);
            });

        public async ValueTask<List<Address>> RetrieveAddressByPostCodeAsync(string postCode) =>
            await this.addressService.RetrieveAddressByPostCodeAsync(postCode);
    }
}
