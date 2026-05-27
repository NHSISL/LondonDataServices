// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService : IIngestionTrackingService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                IngestionTracking ingestionTrackingWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(ingestionTracking);

                await ValidateIngestionTrackingOnAddAsync(ingestionTrackingWithAddAuditApplied);

                return await this.storageBroker.InsertIngestionTrackingAsync(ingestionTrackingWithAddAuditApplied);
            });

        public ValueTask<IQueryable<IngestionTracking>> RetrieveAllIngestionTrackingsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllIngestionTrackingsAsync());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                return maybeIngestionTracking;
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByFileNameAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(fileName);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.storageBroker.SelectAllIngestionTrackingsAsync();

                IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByEncryptedFileNameAsync(
            string encryptedFileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(encryptedFileName);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.storageBroker.SelectAllIngestionTrackingsAsync();

                IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                        .FirstOrDefault(ingestionTracking => ingestionTracking.EncryptedFileName == encryptedFileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                IngestionTracking ingestionTrackingWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(ingestionTracking);

                await ValidateIngestionTrackingOnModifyAsync(ingestionTrackingWithModifyAuditApplied);

                IngestionTracking maybeIngestionTracking =
                    await this.storageBroker.SelectIngestionTrackingByIdAsync(ingestionTracking.Id);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId: ingestionTracking.Id);

                IngestionTracking IngestionTrackingWithModifyAuditAppliedEnsured =
                  await this.securityAuditBroker.EnsureAddAuditValuesRemainsUnchangedOnModifyAsync(
                      ingestionTrackingWithModifyAuditApplied,
                      maybeIngestionTracking);

                ValidateAgainstStorageIngestionTrackingOnModify(
                    inputIngestionTracking: ingestionTrackingWithModifyAuditApplied,
                    storageIngestionTracking: maybeIngestionTracking);

                return await this.storageBroker.UpdateIngestionTrackingAsync(ingestionTrackingWithModifyAuditApplied);
            });

        public ValueTask BulkModifyIngestionTrackingAsync(List<IngestionTracking> ingestionTrackingItems) =>
            TryCatch(async () =>
            {
                await ValidateOnBulkModifyIngestionTrackingAsync(ingestionTrackingItems);
                await BulkAddOrModifyBySplittingIntoBatchesAsync(ingestionTrackingItems);
            });


        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                return await this.storageBroker.DeleteIngestionTrackingAsync(maybeIngestionTracking);
            });

        virtual internal async ValueTask BulkAddOrModifyBySplittingIntoBatchesAsync(
            List<IngestionTracking> ingestionTrackingItems,
            int batchSize = 10000)
        {
            int totalRecords = ingestionTrackingItems.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    List<IngestionTracking> batch = ingestionTrackingItems.Skip(i).Take(batchSize).ToList();
                    List<Guid> batchIds = batch.Select(ingestionTracking => ingestionTracking.Id).ToList();

                    IQueryable<IngestionTracking> storageIngestionTrackings =
                        (await this.storageBroker.SelectAllIngestionTrackingsAsync())
                            .Where(ingestionTracking => batchIds.Contains(ingestionTracking.Id));

                    HashSet<Guid> existingIds = storageIngestionTrackings
                        .Select(ingestionTracking => ingestionTracking.Id).ToHashSet();

                    List<IngestionTracking> existingIngestionTrackings =
                        batch.Where(ingestionTracking => existingIds.Contains(ingestionTracking.Id)).ToList();

                    List<IngestionTracking> newIngestionTrackings =
                        batch.Where(ingestionTracking => !existingIds.Contains(ingestionTracking.Id)).ToList();

                    try
                    {
                        if (newIngestionTrackings.Count != 0)
                        {
                            List<IngestionTracking> validatedAddIngestionTrackings =
                                await AssignAuditValuesAndValidateOnBulkAddAsync(newIngestionTrackings);

                            await this.storageBroker.BulkInsertIngestionTrackingsAsync(
                                validatedAddIngestionTrackings);
                        }
                    }
                    catch (Exception insertException)
                    {
                        exceptions.Add(insertException);
                        await this.loggingBroker.LogErrorAsync(insertException);
                    }

                    try
                    {
                        if (existingIngestionTrackings.Count != 0)
                        {
                            List<IngestionTracking> validatedModifyIngestionTrackings =
                                await AssignAuditValuesAndValidateOnBulkModifyAsync(existingIngestionTrackings);

                            await this.storageBroker.BulkUpdateIngestionTrackingsAsync(
                                validatedModifyIngestionTrackings);
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
                    $"Unable to process ingestionTrackings in {exceptions.Count} of the batch(es)",
                    exceptions);
            }
        }

        virtual internal async ValueTask<List<IngestionTracking>> AssignAuditValuesAndValidateOnBulkAddAsync(
            List<IngestionTracking> ingestionTrackingItems)
        {
            List<IngestionTracking> validatedIngestionTrackings = new List<IngestionTracking>();
            List<Audit> audits = new List<Audit>();

            foreach (IngestionTracking item in ingestionTrackingItems)
            {
                try
                {
                    IngestionTracking appliedAuditItem =
                        await this.securityAuditBroker.ApplyAddAuditValuesAsync(item);

                    await ValidateIngestionTrackingOnAddAsync(item);
                    validatedIngestionTrackings.Add(item);
                }
                catch (Exception exception)
                {
                    Audit audit = new Audit
                    {
                        AuditType = "IngestionTracking",
                        Title = "Unable to add item",

                        Message =
                            $"Invalid ingestion tracking item - Id: {item.Id} " +
                            $"Error: {exception.Message}",

                        LogLevel = "Error",
                    };

                    audits.Add(audit);

                    await this.loggingBroker.LogWarningAsync(message:
                            $"Invalid ingestion tracking item - Id: {item.Id} " + Environment.NewLine +
                            $"Error: {exception.Message}");
                }
            }

            if (audits.Any())
            {
                await this.auditBroker.BulkLogAsync(audits);
            }

            return validatedIngestionTrackings;
        }

        virtual internal async ValueTask<List<IngestionTracking>> AssignAuditValuesAndValidateOnBulkModifyAsync(
            List<IngestionTracking> ingestionTrackingItems)
        {
            List<IngestionTracking> validatedIngestionTrackings = new List<IngestionTracking>();
            List<Audit> audits = new List<Audit>();

            foreach (IngestionTracking item in ingestionTrackingItems)
            {
                try
                {
                    IngestionTracking appliedAuditItem =
                        await this.securityAuditBroker.ApplyModifyAuditValuesAsync(item);

                    await ValidateIngestionTrackingOnModifyAsync(appliedAuditItem);
                    validatedIngestionTrackings.Add(appliedAuditItem);
                }
                catch (Exception exception)
                {
                    Audit audit = new Audit
                    {
                        AuditType = "IngestionTracking",
                        Title = "Unable to update item",

                        Message =
                            $"Invalid ingestion tracking item - Id: {item.Id} " +
                            $"Error: {exception.Message}",

                        LogLevel = "Error",
                    };

                    audits.Add(audit);

                    await this.loggingBroker.LogWarningAsync(message:
                        $"Invalid ingestion tracking item - Id: {item.Id} " + Environment.NewLine +
                        $"Error: {exception.Message}");
                }
            }

            if (audits.Any())
            {
                await this.auditBroker.BulkLogAsync(audits);
            }

            return await ValueTask.FromResult(validatedIngestionTrackings);
        }
    }
}