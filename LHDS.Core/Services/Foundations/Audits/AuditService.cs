// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService : IAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ISecurityBroker securityBroker;

        public AuditService(
            IStorageBroker storageBroker,
            IIdentifierBroker identifierBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.identifierBroker = identifierBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Audit> AddAuditAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId,
            string? logLevel = "Information") =>
        TryCatch(async () =>
        {
            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            Audit audit = new Audit
            {
                Id = await this.identifierBroker.GetIdentifierAsync(),
                AuditType = auditType,
                Title = title,
                Message = message,
                CorrelationId = correlationId,
                FileName = fileName,
                LogLevel = logLevel,
                CreatedBy = "System",
                CreatedDate = dateTimeOffset,
                UpdatedBy = "System",
                UpdatedDate = dateTimeOffset,
            };

            await ValidateAuditOnAddAsync(audit);

            return await this.storageBroker.InsertAuditAsync(audit);
        });

        public ValueTask<Audit> AddAuditAsync(Audit audit) =>
        TryCatch(async () =>
        {
            await ValidateAuditOnAddAsync(audit);

            return await this.storageBroker.InsertAuditAsync(audit);
        });

        public ValueTask BulkAddAuditsAsync(List<Audit> audits, int batchSize = 10000) =>
        TryCatch(async () =>
        {
            ValidateOnBulkAddAudits(audits);
            await BatchBulkAddAuditsAsync(audits, batchSize);
        });

        public ValueTask<IQueryable<Audit>> RetrieveAllAuditsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllAuditsAsync());

        public ValueTask<Audit> RetrieveAuditByIdAsync(Guid auditId) =>
        TryCatch(async () =>
        {
            ValidateAuditId(auditId);

            Audit maybeAudit = await this.storageBroker
                .SelectAuditByIdAsync(auditId);

            ValidateStorageAudit(maybeAudit, auditId);

            return maybeAudit;
        });

        public ValueTask<Audit> ModifyAuditAsync(Audit audit) =>
        TryCatch(async () =>
        {
            await ValidateAuditOnModifyAsync(audit);

            Audit maybeAudit =
                await this.storageBroker.SelectAuditByIdAsync(audit.Id);

            ValidateStorageAudit(maybeAudit, audit.Id);
            ValidateAgainstStorageAuditOnModify(inputAudit: audit, storageAudit: maybeAudit);

            return await this.storageBroker.UpdateAuditAsync(audit);
        });

        public ValueTask<Audit> RemoveAuditByIdAsync(Guid auditId) =>
        TryCatch(async () =>
        {
            ValidateAuditId(auditId);

            Audit maybeAudit = await this.storageBroker
                .SelectAuditByIdAsync(auditId);

            ValidateStorageAudit(maybeAudit, auditId);

            return await this.storageBroker.DeleteAuditAsync(maybeAudit);
        });

        virtual internal async ValueTask BatchBulkAddAuditsAsync(List<Audit> audits, int batchSize)
        {
            int totalRecords = audits.Count;
            var exceptions = new List<Exception>();

            for (int i = 0; i < totalRecords; i += batchSize)
            {
                try
                {
                    var batch = audits.Skip(i).Take(batchSize).ToList();

                    if (batch.Count != 0)
                    {
                        List<Audit> validatedAudits = await ValidateAuditsAndAssignIdAndAuditAsync(batch);
                        await this.storageBroker.BulkInsertAuditsAsync(validatedAudits);
                    }
                }
                catch (Exception ex)
                {
                    await this.loggingBroker.LogErrorAsync(ex);
                }
            }
        }

        virtual internal async ValueTask<List<Audit>> ValidateAuditsAndAssignIdAndAuditAsync(List<Audit> audits)
        {
            List<Audit> validatedAudites = new List<Audit>();

            foreach (Audit address in audits)
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
                    await ValidateAuditOnAddAsync(address);
                    validatedAudites.Add(address);
                }
                catch (Exception ex)
                {
                    await this.loggingBroker.LogErrorAsync(ex);
                }
            }

            return await ValueTask.FromResult(validatedAudites);
        }
    }
}