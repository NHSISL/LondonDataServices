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
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService : IOptOutService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IAuditBroker auditBroker;

        public OptOutService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker,
            IAuditBroker auditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
            this.auditBroker = auditBroker;
        }

        public ValueTask<OptOut> AddOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                OptOut optOutWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(optOut);

                await ValidateOptOutOnAddAsync(optOutWithAddAuditApplied);

                return await this.storageBroker.InsertOptOutAsync(optOutWithAddAuditApplied);
            });

        public ValueTask<IQueryable<OptOut>> RetrieveAllOptOutsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllOptOutsAsync());

        public ValueTask<OptOut> RetrieveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                OptOut maybeOptOut = await this.storageBroker
                    .SelectOptOutByIdAsync(optOutId);

                ValidateStorageOptOut(maybeOptOut, optOutId);

                return maybeOptOut;
            });

        public ValueTask<OptOut> ModifyOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                OptOut optOutWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(optOut);

                await ValidateOptOutOnModifyAsync(optOutWithModifyAuditApplied);

                OptOut maybeOptOut =
                    await this.storageBroker.SelectOptOutByIdAsync(optOut.Id);

                ValidateStorageOptOut(maybeOptOut, optOut.Id);
                ValidateAgainstStorageOptOutOnModify(inputOptOut: optOut, storageOptOut: maybeOptOut);

                return await this.storageBroker.UpdateOptOutAsync(optOut);
            });

        public ValueTask<OptOut> RemoveOptOutByIdAsync(Guid optOutId) =>
            TryCatch(async () =>
            {
                ValidateOptOutId(optOutId);

                OptOut maybeOptOut = await this.storageBroker
                    .SelectOptOutByIdAsync(optOutId);

                ValidateStorageOptOut(maybeOptOut, optOutId);

                return await this.storageBroker.DeleteOptOutAsync(maybeOptOut);
            });

        public ValueTask BulkAddOptOutsAsync(List<OptOut> optOuts, string fileName) =>
            TryCatch(async () =>
            {
                ValidateOnBulkAddOptOuts(optOuts, fileName);
                await BulkAddOrModifyBatchAsync(optOuts, fileName, 10000);
            });

        public ValueTask BulkModifyOptOutsAsync(
            List<OptOut> optOuts,
            string fileName) =>
                TryCatch(async () =>
                {
                    ValidateOnBulkModifyOptOuts(optOuts, fileName);
                    await BulkAddOrModifyBatchAsync(optOuts, fileName, 10000, allowInserts: false);
                });

        internal virtual async ValueTask BulkAddOrModifyBatchAsync(
            List<OptOut> optOuts,
            string fileName,
            int batchSize = 10000,
            bool allowInserts = true)
        {
            int totalRecords = optOuts.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    List<OptOut> batch =
                        optOuts.Skip(i).Take(batchSize).ToList();

                    List<Guid> batchIds =
                        batch.Select(optOut => optOut.Id).ToList();

                    IQueryable<OptOut> storageOptOuts =
                        (await this.storageBroker.SelectAllOptOutsAsync())
                            .Where(optOut => batchIds.Contains(optOut.Id));

                    List<Guid> existingIds =
                        storageOptOuts.Select(optOut => optOut.Id).ToList();

                    List<OptOut> existingOptOuts = batch
                        .Where(optOut => existingIds.Contains(optOut.Id))
                        .ToList();

                    List<OptOut> newOptOuts = batch
                        .Where(optOut => !existingIds.Contains(optOut.Id))
                        .ToList();

                    try
                    {
                        if (newOptOuts.Count != 0)
                        {
                            if (allowInserts)
                            {
                                List<OptOut> validatedAddOptOuts =
                                    await ValidateOptOutsAndAssignIdAndAuditOnAddAsync(
                                        newOptOuts, fileName);

                                await this.storageBroker
                                    .BulkInsertOptOutsAsync(validatedAddOptOuts);
                            }
                            else
                            {
                                List<Audit> skippedAudits = newOptOuts
                                    .Select(optOut => new Audit
                                    {
                                        AuditType = "OptOut",
                                        Title = "Unable to modify optOut",

                                        Message =
                                            $"OptOut not found in storage - Id: {optOut.Id};"
                                                + $" NhsNumber: {optOut.NhsNumber}"
                                                + $" from file: {fileName}",

                                        FileName = fileName,
                                        LogLevel = "Error",
                                    })
                                    .ToList();

                                await this.auditBroker.BulkLogAsync(skippedAudits);

                                foreach (OptOut optOut in newOptOuts)
                                {
                                    await this.loggingBroker.LogWarningAsync(
                                        message: $"Unable to modify optOut."
                                            + $" OptOut not found in storage - Id: {optOut.Id};"
                                            + $" NhsNumber: {optOut.NhsNumber}"
                                            + $" from file: {fileName}");
                                }
                            }
                        }
                    }
                    catch (Exception insertException)
                    {
                        exceptions.Add(insertException);
                        await this.loggingBroker.LogErrorAsync(insertException);
                    }

                    try
                    {
                        if (existingOptOuts.Count != 0)
                        {
                            Dictionary<Guid, OptOut> storageById =
                                storageOptOuts.ToDictionary(optOut => optOut.Id);

                            foreach (OptOut optOut in existingOptOuts)
                            {
                                if (storageById.TryGetValue(optOut.Id, out OptOut stored))
                                {
                                    optOut.CreatedDate = stored.CreatedDate;
                                    optOut.CreatedBy = stored.CreatedBy;
                                }
                            }

                            List<OptOut> validatedModifyOptOuts =
                                await ValidateOptOutsAndAssignAuditOnModifyAsync(
                                    existingOptOuts, fileName);

                            await this.storageBroker
                                .BulkUpdateOptOutsAsync(validatedModifyOptOuts);
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
                    $"Unable to process optOuts in {exceptions.Count}"
                        + $" of the batch(es) from {fileName}",
                    exceptions);
            }
        }

        internal virtual async ValueTask<List<OptOut>>
            ValidateOptOutsAndAssignIdAndAuditOnAddAsync(
                List<OptOut> optOuts,
                string fileName)
        {
            List<OptOut> validatedOptOuts = new List<OptOut>();
            List<Audit> audits = new List<Audit>();

            foreach (OptOut optOut in optOuts)
            {
                try
                {
                    string currentUserId =
                        await this.securityAuditBroker.GetUserIdAsync();

                    var currentDateTime =
                        await this.dateTimeBroker
                            .GetCurrentDateTimeOffsetAsync();

                    optOut.Id =
                        await this.identifierBroker.GetIdentifierAsync();

                    optOut.CreatedDate = currentDateTime;
                    optOut.CreatedBy = currentUserId;
                    optOut.UpdatedDate = currentDateTime;
                    optOut.UpdatedBy = currentUserId;
                    await ValidateOptOutOnAddAsync(optOut);
                    validatedOptOuts.Add(optOut);
                }
                catch (Exception exception)
                {
                    Audit audit = new Audit
                    {
                        AuditType = "OptOut",
                        Title = "Unable to add optOut",

                        Message =
                            $"Invalid optOut - Id: {optOut?.Id};"
                                + $" NhsNumber: {optOut?.NhsNumber}"
                                + $" from file: {fileName}"
                                + Environment.NewLine
                                + $"Error: {exception.Message}",

                        FileName = fileName,
                        LogLevel = "Error",
                    };

                    audits.Add(audit);

                    await this.loggingBroker.LogWarningAsync(
                        message: $"Unable to add optOut."
                            + $" Invalid optOut - Id: {optOut?.Id};"
                            + $" NhsNumber: {optOut?.NhsNumber}"
                            + $" from file: {fileName}"
                            + Environment.NewLine
                            + $"Error: {exception.Message}");
                }
            }

            if (audits.Any())
            {
                await this.auditBroker.BulkLogAsync(audits);
            }

            return await ValueTask.FromResult(validatedOptOuts);
        }

        internal virtual async ValueTask<List<OptOut>>
            ValidateOptOutsAndAssignAuditOnModifyAsync(
                List<OptOut> optOuts,
                string fileName)
        {
            List<OptOut> validatedOptOuts = new List<OptOut>();
            List<Audit> audits = new List<Audit>();

            foreach (OptOut optOut in optOuts)
            {
                try
                {
                    string currentUserId =
                        await this.securityAuditBroker.GetUserIdAsync();

                    var currentDateTime =
                        await this.dateTimeBroker
                            .GetCurrentDateTimeOffsetAsync();

                    optOut.UpdatedDate = currentDateTime;
                    optOut.UpdatedBy = currentUserId;
                    await ValidateOptOutOnModifyAsync(optOut);
                    validatedOptOuts.Add(optOut);
                }
                catch (Exception exception)
                {
                    Audit audit = new Audit
                    {
                        AuditType = "OptOut",
                        Title = "Unable to modify optOut",

                        Message =
                            $"Invalid optOut - Id: {optOut?.Id};"
                                + $" NhsNumber: {optOut?.NhsNumber}"
                                + $" from file: {fileName}"
                                + Environment.NewLine
                                + $"Error: {exception.Message}",

                        FileName = fileName,
                        LogLevel = "Error",
                    };

                    audits.Add(audit);

                    await this.loggingBroker.LogWarningAsync(
                        message: $"Unable to modify optOut."
                            + $" Invalid optOut - Id: {optOut?.Id};"
                            + $" NhsNumber: {optOut?.NhsNumber}"
                            + $" from file: {fileName}"
                            + Environment.NewLine
                            + $"Error: {exception.Message}");
                }
            }

            if (audits.Any())
            {
                await this.auditBroker.BulkLogAsync(audits);
            }

            return await ValueTask.FromResult(validatedOptOuts);
        }
    }
}