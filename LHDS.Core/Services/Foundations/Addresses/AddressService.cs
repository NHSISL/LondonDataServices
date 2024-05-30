// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
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
        private readonly IAuditBroker auditBroker;

        public AddressService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                ValidateAddressOnAdd(address);

                return await this.storageBroker.InsertAddressAsync(address);
            });

        public ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName) =>
            TryCatch(async () =>
            {
                List<Address> validatedAddresses = new List<Address>();
                await ExtractValidAddresses(addresses, fileName, validatedAddresses);
                int batchSize = 10000;
                await BulkInsertBatch(validatedAddresses, batchSize);
            });

        private async Task BulkInsertBatch(List<Address> validatedAddresses, int batchSize)
        {
            for (int i = 0; i < validatedAddresses.Count; i += batchSize)
            {
                var batch = validatedAddresses.Skip(i).Take(batchSize).ToList();

                var stopwatch = Stopwatch.StartNew();

                var existingUPRNs = this.storageBroker.SelectAllAddresses()
                    .Where(address => batch.Any(validatedAddress => validatedAddress.UPRN == address.UPRN))
                    .Select(address => address.UPRN)
                    .ToList();

                var newAddresses = batch.Where(address => !existingUPRNs.Contains(address.UPRN)).ToList();

                foreach (var newAddress in newAddresses)
                {
                    newAddress.Id = Guid.NewGuid();
                    newAddress.CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    newAddress.CreatedBy = "System";
                    newAddress.UpdatedDate = newAddress.CreatedDate;
                    newAddress.UpdatedBy = newAddress.CreatedBy;
                }

                await this.storageBroker.BulkInsertAddressesAsync(newAddresses);

                stopwatch.Stop(); // Stop the stopwatch
                Console.WriteLine($"Batch processed in {stopwatch.ElapsedMilliseconds} milliseconds");
            }
        }

        private async Task ExtractValidAddresses(List<Address> addresses, string fileName, List<Address> validatedAddresses)
        {
            foreach (Address address in addresses)
            {
                try
                {
                    ValidateAddressOnAdd(address);
                    validatedAddresses.Add(address);
                }
                catch (Exception ex)
                {
                    int indexOfInvalidItem = addresses.IndexOf(address);

                    if (indexOfInvalidItem != -1)
                    {
                        await this.auditBroker.LogWarning(
                            auditType: "Address",
                            title: "Invalid address parts found",
                            message: $"Invalid address parts found in line item: {indexOfInvalidItem + 1} " +
                                     $"from file: {fileName}",
                            fileName,
                            null);
                    }
                }
            }
        }

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

        public ValueTask<List<Address>> RetrieveAddressesByPostCodeAsync(string postCode) =>
            TryCatch(async () =>
            {
                ValidatePostCode(postCode);

                List<Address> returnedAddresses = this.storageBroker
                    .SelectAllAddresses()
                    .ToList()
                    .Where(address => address.PostCode.Equals(postCode, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return await ValueTask.FromResult(returnedAddresses);
            });
    }
}