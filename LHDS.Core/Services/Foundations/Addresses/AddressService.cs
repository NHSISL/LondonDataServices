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
                Console.WriteLine($"{addresses.Count()} records to process");
                var stopwatch = Stopwatch.StartNew();
                Console.WriteLine($"Batch processed in {stopwatch.ElapsedMilliseconds} milliseconds");
                int batchSize = 10000;
                await BulkInsertBatch(addresses, batchSize, fileName);
                stopwatch.Stop();
                Console.WriteLine($"Batch processed in {stopwatch.ElapsedMilliseconds} milliseconds");
            });

        private async ValueTask BulkInsertBatch(List<Address> addresses, int batchSize, string fileName)
        {
            int totalRecords = addresses.Count;
            int processedRecords = 0;

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    var batch = addresses.Skip(i).Take(batchSize).ToList();
                    List<Address> validatedAddresses = await ExtractValidAddresses(batch, fileName);
                    var stopwatch = Stopwatch.StartNew();
                    var batchUPRNs = batch.Select(validatedAddress => validatedAddress.UPRN).ToList();

                    var existingUPRNs = this.storageBroker.SelectAllAddresses()
                        .Where(address => batchUPRNs.Contains(address.UPRN))
                        .Select(address => address.UPRN)
                        .ToList();

                    var newAddresses = batch.Where(address => !existingUPRNs.Contains(address.UPRN)).ToList();

                    if (newAddresses.Count != 0)
                    {
                        await this.storageBroker.BulkInsertAddressesAsync(newAddresses);
                    }

                    stopwatch.Stop(); // Stop the stopwatch

                    processedRecords += batch.Count;

                    Console.WriteLine(
                        $"Batch processed in {stopwatch.ElapsedMilliseconds} milliseconds. " + Environment.NewLine +
                        $"Processed {processedRecords} out of {totalRecords} records. " + Environment.NewLine +
                        $"{totalRecords - processedRecords} records remaining.");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private async ValueTask<List<Address>> ExtractValidAddresses(List<Address> addresses, string fileName)
        {
            List<Address> validatedAddresses = new List<Address>();

            foreach (Address address in addresses)
            {
                try
                {
                    address.Id = Guid.NewGuid();
                    address.CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
                    address.CreatedBy = "System";
                    address.UpdatedDate = address.CreatedDate;
                    address.UpdatedBy = address.CreatedBy;
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

            return await ValueTask.FromResult(validatedAddresses);
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