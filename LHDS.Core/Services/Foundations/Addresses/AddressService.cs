// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService : IAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public AddressService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                await ValidateAddressOnAddAsync(address);

                return await this.storageBroker.InsertAddressAsync(address);
            });

        public ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateOnBulkAddAddresses(addresses, fileName);
                int batchSize = 10000;
                await BulkInsertBatch(addresses, batchSize, fileName);
            });

        virtual internal async ValueTask BulkInsertBatch(List<Address> addresses, int batchSize, string fileName)
        {
            int totalRecords = addresses.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    await TryCatch(async () =>
                    {
                        var batch = addresses.Skip(i).Take(batchSize).ToList();

                        List<Address> validatedAddresses = 
                            await ExtractValidAddressesAndAssignIdAndAuditAsync(batch, fileName);

                        var batchUPRNs = batch.Select(validatedAddress => validatedAddress.UPRN).ToList();
                        var allAddresses = await this.storageBroker.SelectAllAddressesAsync();

                        var existingUPRNs = allAddresses
                            .Where(address => batchUPRNs.Contains(address.UPRN))
                            .Select(address => address.UPRN)
                            .ToList();

                        var newAddresses = batch.Where(address => !existingUPRNs.Contains(address.UPRN)).ToList();

                        if (newAddresses.Count != 0)
                        {
                            await this.storageBroker.BulkInsertAddressesAsync(newAddresses);
                        }
                    });
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to process addresses in {exceptions.Count} of the batch(es) from {fileName}",
                    exceptions);
            }
        }

        virtual internal async ValueTask<List<Address>> ExtractValidAddressesAndAssignIdAndAuditAsync(
            List<Address> addresses,
            string fileName)
        {
            List<Address> validatedAddresses = new List<Address>();

            foreach (Address address in addresses)
            {
                try
                {
                    var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                    address.Id = await this.identifierBroker.GetIdentifierAsync();
                    address.CreatedDate = currentDateTime;
                    address.CreatedBy = "System";
                    address.UpdatedDate = address.CreatedDate;
                    address.UpdatedBy = address.CreatedBy;
                    await ValidateAddressOnAddAsync(address);
                    validatedAddresses.Add(address);
                }
                catch (Exception ex)
                {
                    if (ex is NullAddressException || ex is InvalidAddressException)
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

                    throw;
                }
            }

            return await ValueTask.FromResult(validatedAddresses);
        }

        public ValueTask<IQueryable<Address>> RetrieveAllAddressesAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllAddressesAsync());

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
                await ValidateAddressOnModifyAsync(address);

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
                IQueryable<Address> allreturnedAddresses = await this.storageBroker.SelectAllAddressesAsync();

                List<Address> matchedAddresses = allreturnedAddresses
                    .Where(address => address.PostCode.Equals(postCode, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return matchedAddresses;
            });
    }
}