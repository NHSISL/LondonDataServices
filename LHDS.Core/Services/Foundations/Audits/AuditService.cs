// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService : IAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

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

                Audit auditWithAddAuditApplied = await ApplyAddAuditAsync(audit);
                await ValidateAuditOnAddAsync(auditWithAddAuditApplied);

                return await this.storageBroker.InsertAuditAsync(auditWithAddAuditApplied);
            });

        public ValueTask<Audit> AddAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                Audit auditWithAddAuditApplied = await ApplyAddAuditAsync(audit);
                await ValidateAuditOnAddAsync(auditWithAddAuditApplied);

                return await this.storageBroker.InsertAuditAsync(auditWithAddAuditApplied);
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
                Audit auditWithModifyAuditApplied = await ApplyModifyAuditAsync(audit);
                await ValidateAuditOnModifyAsync(auditWithModifyAuditApplied);
                Audit maybeAudit =  await this.storageBroker.SelectAuditByIdAsync(audit.Id);
                ValidateStorageAudit(maybeAudit, audit.Id);
                ValidateAgainstStorageAuditOnModify(inputAudit: audit, storageAudit: maybeAudit);

                return await this.storageBroker.UpdateAuditAsync(auditWithModifyAuditApplied);
            });

        public ValueTask<Audit> RemoveAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateAuditId(auditId: auditId);

                Audit maybeAudit = await this.storageBroker
                    .SelectAuditByIdAsync(auditId);

                ValidateStorageAudit(maybeAudit, auditId);

                Audit auditWithDeleteAuditApplied =
                    await ApplyDeleteAuditAsync(maybeAudit);

                Audit updatedAudit =
                    await this.storageBroker.UpdateAuditAsync(auditWithDeleteAuditApplied);

                await ValidateAgainstStorageAuditOnDeleteAsync(
                    audit: updatedAudit,
                    maybeAudit: auditWithDeleteAuditApplied);

                return await this.storageBroker.DeleteAuditAsync(updatedAudit);
            });

        virtual internal async ValueTask<Audit> ApplyAddAuditAsync(Audit audit)
        {
            ValidateAuditIsNotNull(audit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            audit.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            audit.CreatedDate = auditDateTimeOffset;
            audit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            audit.UpdatedDate = auditDateTimeOffset;

            return audit;
        }

        virtual internal async ValueTask<Audit> ApplyModifyAuditAsync(Audit audit)
        {
            ValidateAuditIsNotNull(audit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            audit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            audit.UpdatedDate = auditDateTimeOffset;

            return audit;
        }

        virtual internal async ValueTask<Audit> ApplyDeleteAuditAsync(Audit audit)
        {
            ValidateAuditIsNotNull(audit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            audit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            audit.UpdatedDate = auditDateTimeOffset;

            return audit;
        }
    }
}