// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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

        public ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateArguments(addresses, fileName);

                await this.addressService.BulkAddAddressesAsync(addresses, fileName);
            });

        public ValueTask BulkModifyAddressesAsync(List<Address> addresses, string fileName) =>
            throw new NotImplementedException();

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddress(address);

                return await this.addressService.AddAddressAsync(address);
            });

        public ValueTask<IQueryable<Address>> RetrieveAllAddressesAsync() =>
            TryCatch(async () => await this.addressService.RetrieveAllAddressesAsync());

        public ValueTask<Address> RetrieveAddressByIdAsync(Guid addressId) =>
            TryCatch(async () =>
            {
                ValidateAddressId(addressId);

                return await this.addressService.RetrieveAddressByIdAsync(addressId);
            });

        public ValueTask<Address> RetrieveOrAddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddressOnRetrieveOrAdd(address);

                return await this.addressService.RetrieveAddressByIdAsync(address.Id) ??
                    await this.addressService.AddAddressAsync(address);
            });

        public ValueTask<Address> ModifyOrAddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddressOnModifyOrAdd(address);
                var allAddresses = await this.addressService.RetrieveAllAddressesAsync();
                var maybeAddress = allAddresses.FirstOrDefault(storageAddress => storageAddress.Id == address.Id);

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

        public ValueTask<List<Address>> RetrieveAddressesByPostCodeAsync(string postCode) =>
            TryCatch(async () =>
            {
                ValidatePostCode(postCode);

                return await this.addressService.RetrieveAddressesByPostCodeAsync(postCode);
            });

        public ValueTask<Address?> RetrieveAddressByUPRNAsync(string uprn) =>
             TryCatch(async () =>
             {
                 ValidateUPRN(uprn);
                 var allAddress = await this.addressService.RetrieveAllAddressesAsync();

                 return allAddress.FirstOrDefault(storageAddress => storageAddress.UPRN == uprn);
             });
    }
}
