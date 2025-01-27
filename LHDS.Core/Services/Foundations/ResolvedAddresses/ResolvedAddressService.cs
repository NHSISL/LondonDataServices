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
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService : IResolvedAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public ResolvedAddressService(
            IStorageBroker storageBroker,
            IIdentifierBroker identifierBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.identifierBroker = identifierBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                await ValidateResolvedAddressOnAddAsync(resolvedAddress);

                return await this.storageBroker.InsertResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask BulkAddResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateOnBulkAddResolvedAddresses(resolvedAddresses, fileName);
                int batchSize = 10000;
                await BulkInsertBatch(resolvedAddresses, batchSize, fileName);
            });

        public ValueTask<IQueryable<ResolvedAddress>> RetrieveAllResolvedAddressesAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllResolvedAddressesAsync());

        public ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                ResolvedAddress maybeResolvedAddress = await this.storageBroker
                    .SelectResolvedAddressByIdAsync(resolvedAddressId);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddressId);

                return maybeResolvedAddress;
            });

        public ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                await ValidateResolvedAddressOnModifyAsync(resolvedAddress);

                ResolvedAddress maybeResolvedAddress =
                    await this.storageBroker.SelectResolvedAddressByIdAsync(resolvedAddress.Id);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddress.Id);
                ValidateAgainstStorageResolvedAddressOnModify(inputResolvedAddress: resolvedAddress, storageResolvedAddress: maybeResolvedAddress);

                return await this.storageBroker.UpdateResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask BulkModifyResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses) =>
            TryCatch(async () =>
            {
                ValidateOnBulkModifyResolvedAddresses(resolvedAddresses);
                int batchSize = 10000;
                await BulkUpdateBatch(resolvedAddresses, batchSize);
            });

        public ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                ResolvedAddress maybeResolvedAddress = await this.storageBroker
                    .SelectResolvedAddressByIdAsync(resolvedAddressId);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddressId);

                return await this.storageBroker.DeleteResolvedAddressAsync(maybeResolvedAddress);
            });

        virtual internal async ValueTask BulkInsertBatch(
            List<ResolvedAddress> resolvedAddresses,
            int batchSize,
            string fileName)
        {
            int totalRecords = resolvedAddresses.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    await TryCatch(async () =>
                    {
                        var batch = resolvedAddresses.Skip(i).Take(batchSize).ToList();

                        List<ResolvedAddress> validatedResolvedAddresses =
                            await ApplyIdAndAuditAndValidResolvedAddresses(batch, fileName);

                        var referencesFromValidatedResolvedAddresses =
                            batch.Select(validatedResolvedAddress =>
                                validatedResolvedAddress.UniqueReference).ToList();

                        var retrievedResolvedAddresses = await this.storageBroker.SelectAllResolvedAddressesAsync();

                        var existingUniqueReferencesToExclude = retrievedResolvedAddresses
                            .Where(resolvedAddress => referencesFromValidatedResolvedAddresses
                                .Contains(resolvedAddress.UniqueReference))

                            .Select(resolvedAddress => resolvedAddress.UPRN)
                            .ToList();

                        var newResolvedAddresses = batch.Where(address =>
                            !existingUniqueReferencesToExclude.Contains(address.UPRN)).ToList();

                        if (newResolvedAddresses.Count != 0)
                        {
                            await this.storageBroker.BulkInsertResolvedAddressesAsync(newResolvedAddresses);
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
                    $"Unable to process resolved addresses in {exceptions.Count} of the batch(es) from {fileName}",
                    exceptions);
            }
        }

        virtual internal async ValueTask BulkUpdateBatch(
            List<ResolvedAddress> resolvedAddresses,
            int batchSize)
        {
            int totalRecords = resolvedAddresses.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    await TryCatch(async () =>
                    {
                        var batch = resolvedAddresses.Skip(i).Take(batchSize).ToList();

                        List<ResolvedAddress> validatedResolvedAddresses =
                            await ApplyAuditAndValidResolvedAddresses(batch);

                        var idsFromValidatedResolvedAddresses =
                            batch.Select(validatedResolvedAddress =>
                                validatedResolvedAddress.Id).ToList();

                        var retrievedResolvedAddresses = await this.storageBroker.SelectAllResolvedAddressesAsync();

                        var existingIdsInDatabase = retrievedResolvedAddresses
                            .Where(resolvedAddress => idsFromValidatedResolvedAddresses
                                .Contains(resolvedAddress.Id))

                            .Select(resolvedAddress => resolvedAddress.Id)
                            .ToList();

                        var toUpdateResolvedAddresses = batch.Where(resolvedAddress =>
                            existingIdsInDatabase.Contains(resolvedAddress.Id)).ToList();

                        if (toUpdateResolvedAddresses.Count != 0)
                        {
                            await this.storageBroker.BulkUpdateResolvedAddressesAsync(toUpdateResolvedAddresses);
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
                    $"Unable to process resolved addresses in {exceptions.Count} of the batch(es)",
                    exceptions);
            }
        }

        virtual internal async ValueTask<List<ResolvedAddress>> ApplyIdAndAuditAndValidResolvedAddresses(
            List<ResolvedAddress> resolvedAddresses,
            string fileName)
        {
            List<ResolvedAddress> validatedResolvedAddresses = new List<ResolvedAddress>();

            foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
            {
                try
                {
                    var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    resolvedAddress.Id = await this.identifierBroker.GetIdentifierAsync();
                    resolvedAddress.CreatedDate = currentDateTime;
                    resolvedAddress.CreatedBy = "System";
                    resolvedAddress.UpdatedDate = resolvedAddress.CreatedDate;
                    resolvedAddress.UpdatedBy = resolvedAddress.CreatedBy;
                    await ValidateResolvedAddressOnAddAsync(resolvedAddress);
                    validatedResolvedAddresses.Add(resolvedAddress);
                }
                catch (Exception ex)
                {
                    if (ex is NullResolvedAddressException || ex is InvalidResolvedAddressException)
                    {
                        int indexOfInvalidItem = resolvedAddresses.IndexOf(resolvedAddress);

                        if (indexOfInvalidItem != -1)
                        {
                            await this.auditBroker.LogWarning(
                                auditType: "ResolvedAddress",
                                title: "Invalid resolved address parts found",
                                message: $"Invalid address parts found in line item: {indexOfInvalidItem + 1} " +
                                         $"from file: {fileName}",
                                fileName,
                                null);
                        }
                    }

                    throw;
                }
            }

            return await ValueTask.FromResult(validatedResolvedAddresses);
        }

        virtual internal async ValueTask<List<ResolvedAddress>> ApplyAuditAndValidResolvedAddresses(
            List<ResolvedAddress> resolvedAddresses)
        {
            List<ResolvedAddress> validatedResolvedAddresses = new List<ResolvedAddress>();

            foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
            {
                try
                {
                    var currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    resolvedAddress.UpdatedDate = currentDateTime;
                    await ValidateResolvedAddressOnModifyAsync(resolvedAddress);
                    validatedResolvedAddresses.Add(resolvedAddress);
                }
                catch (Exception ex)
                {
                    if (ex is NullResolvedAddressException || ex is InvalidResolvedAddressException)
                    {
                        int indexOfInvalidItem = resolvedAddresses.IndexOf(resolvedAddress);

                        if (indexOfInvalidItem != -1)
                        {
                            await this.auditBroker.LogWarning(
                                auditType: "ResolvedAddress",
                                title: $"Invalid resolved address parts found for Id: {resolvedAddress.Id}",
                                message: $"Invalid address parts found for Id: {resolvedAddress.Id} ",
                                fileName: null,
                                correlationId: resolvedAddress.Id.ToString());
                        }
                    }

                    throw;
                }
            }

            return await ValueTask.FromResult(validatedResolvedAddresses);
        }
    }
}