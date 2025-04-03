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
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService : IAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public AddressService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask<Address> AddAddressAsync(Address address) =>
            TryCatch(async () =>
            {
                Address addressWithAddAuditApplied = await ApplyAddAuditAsync(address);
                await ValidateAddressOnAddAsync(addressWithAddAuditApplied);

                return await this.storageBroker.InsertAddressAsync(address);
            });

        public ValueTask BulkAddAddressesAsync(List<Address> addresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateOnBulkAddAddresses(addresses, fileName);
                await BulkAddOrModifyBatchAsync(addresses, fileName);
            });

        public ValueTask BulkModifyAddressesAsync(List<Address> addresses, string fileName) =>
        TryCatch(async () =>
        {
            ValidateOnBulkModifyAddresses(addresses, fileName);
            await BulkAddOrModifyBatchAsync(addresses, fileName);
        });



        virtual internal async ValueTask BulkAddOrModifyBatchAsync(List<Address> addresses, string fileName, int batchSize = 10000)
        {
            int totalRecords = addresses.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    List<Address> batch = addresses.Skip(i).Take(batchSize).ToList();
                    List<Guid> batchIds = batch.Select(address => address.Id).ToList();

                    IQueryable<Address> storageAddresses = (await this.storageBroker.SelectAllAddressesAsync())
                        .Where(address => batchIds.Contains(address.Id));

                    List<Guid> existingIds = storageAddresses.Select(address => address.Id).ToList();
                    List<Address> existingAddresses = batch.Where(address => existingIds.Contains(address.Id)).ToList();
                    List<Address> newAddresses = batch.Where(address => !existingIds.Contains(address.Id)).ToList();

                    try
                    {
                        if (newAddresses.Count != 0)
                        {
                            List<Address> validatedAddAddresses =
                                await ValidateAddressesAndAssignIdAndAuditOnAddAsync(newAddresses, fileName);

                            await this.storageBroker.BulkInsertAddressesAsync(validatedAddAddresses);
                        }
                    }
                    catch (Exception insertException)
                    {
                        exceptions.Add(insertException);
                        await this.loggingBroker.LogErrorAsync(insertException);
                    }

                    try
                    {
                        if (existingAddresses.Count != 0)
                        {
                            List<Address> validatedModifyAddresses =
                                await ValidateAddressesAndAssignAuditOnModifyAsync(existingAddresses, fileName);

                            await this.storageBroker.BulkUpdateAddressesAsync(validatedModifyAddresses);
                        }
                    }
                    catch (Exception updateException)
                    {
                        exceptions.Add(updateException);
                        await this.loggingBroker.LogErrorAsync(updateException);
                    }
                }
                catch (Exception exception)
                {
                    exceptions.Add(exception);
                    await this.loggingBroker.LogErrorAsync(exception);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to process addresses in {exceptions.Count} of the batch(es) from {fileName}",
                    exceptions);
            }
        }

        virtual internal async ValueTask<List<Address>> ValidateAddressesAndAssignIdAndAuditOnAddAsync(
            List<Address> addresses,
            string fileName)
        {
            List<Address> validatedAddresses = new List<Address>();

            foreach (Address address in addresses)
            {
                try
                {
                    EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();
                    var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    address.Id = await this.identifierBroker.GetIdentifierAsync();
                    address.CreatedDate = currentDateTime;
                    address.CreatedBy = currentUser.EntraUserId;
                    address.UpdatedDate = address.CreatedDate;
                    address.UpdatedBy = address.CreatedBy;
                    await ValidateAddressOnAddAsync(address);
                    validatedAddresses.Add(address);
                }
                catch (Exception exception)
                {
                    if (exception is NullAddressException || exception is InvalidAddressException)
                    {
                        await this.auditBroker.LogWarningAsync(
                            auditType: "Address",
                            title: "Unable to add address",
                            message: $"Invalid address - Id: {address.Id}; UPRN: {address.UPRN}; IPSN: {address.UPSN} " +
                                        $"from file: {fileName}",
                            fileName,
                            null);

                        await this.loggingBroker.LogWarningAsync(message:
                            $"Unable to add address. Invalid address - Id: {address.Id}; UPRN: {address.UPRN}; IPSN: {address.UPSN} " +
                                $"from file: {fileName}");
                    }

                    throw;
                }
            }

            return await ValueTask.FromResult(validatedAddresses);
        }

        virtual internal async ValueTask<List<Address>> ValidateAddressesAndAssignAuditOnModifyAsync(
            List<Address> addresses,
            string fileName)
        {
            List<Address> validatedAddresses = new List<Address>();

            foreach (Address address in addresses)
            {
                try
                {
                    EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();
                    var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    address.CreatedDate = currentDateTime;
                    address.CreatedBy = currentUser.EntraUserId;
                    address.UpdatedDate = address.CreatedDate;
                    address.UpdatedBy = address.CreatedBy;
                    await ValidateAddressOnModifyAsync(address);
                    validatedAddresses.Add(address);
                }
                catch (Exception exception)
                {
                    if (exception is NullAddressException || exception is InvalidAddressException)
                    {
                        int indexOfInvalidItem = addresses.IndexOf(address);

                        if (indexOfInvalidItem != -1)
                        {
                            await this.auditBroker.LogWarningAsync(
                                auditType: "Address",
                                title: "Unable to update address",
                                message: $"Invalid address - Id: {address.Id}; UPRN: {address.UPRN}; IPSN: {address.UPSN} " +
                                         $"from file: {fileName}",
                                fileName,
                                null);

                            await this.loggingBroker.LogWarningAsync(message:
                                $"Unable to modify address. Invalid address - Id: {address.Id}; UPRN: {address.UPRN}; IPSN: {address.UPSN} " +
                                    $"from file: {fileName}");
                        }
                    }

                    throw;
                }
            }

            return await ValueTask.FromResult(validatedAddresses);
        }

        public ValueTask<IQueryable<Address>> RetrieveAllAddressesAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllAddressesAsync());

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
                Address addressWithModifyAuditApplied = await ApplyModifyAuditAsync(address);
                await ValidateAddressOnModifyAsync(address);
                Address maybeAddress = await this.storageBroker.SelectAddressByIdAsync(address.Id);
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

        virtual internal async ValueTask<Address> ApplyAddAuditAsync(
            Address csvIdentificationRequest)
        {
            ValidateAddressIsNotNull(csvIdentificationRequest);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            csvIdentificationRequest.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            csvIdentificationRequest.CreatedDate = auditDateTimeOffset;
            csvIdentificationRequest.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            csvIdentificationRequest.UpdatedDate = auditDateTimeOffset;

            return csvIdentificationRequest;
        }

        virtual internal async ValueTask<Address> ApplyModifyAuditAsync(
            Address address)
        {
            ValidateAddressIsNotNull(address);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            address.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            address.UpdatedDate = auditDateTimeOffset;

            return address;
        }
    }
}