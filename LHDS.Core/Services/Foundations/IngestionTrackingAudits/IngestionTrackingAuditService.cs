// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;

namespace LHDS.Core.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditService : IIngestionTrackingAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IngestionTrackingAudit> AddIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditOnAdd(audit);

                return await this.storageBroker.InsertIngestionTrackingAuditAsync(audit);
            });

        public IQueryable<IngestionTrackingAudit> RetrieveAllIngestionTrackingAudits() =>
            TryCatch(() => this.storageBroker.SelectAllIngestionTrackingAudits());

        public ValueTask<IngestionTrackingAudit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                IngestionTrackingAudit maybeAudit = await this.storageBroker
                    .SelectIngestionTrackingAuditByIdAsync(auditId);

                ValidateStorageIngestionTrackingAudit(maybeAudit, auditId);

                return maybeAudit;
            });

        public ValueTask<IngestionTrackingAudit> ModifyIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditOnModify(audit);

                IngestionTrackingAudit maybeAudit =
                    await this.storageBroker.SelectIngestionTrackingAuditByIdAsync(audit.Id);

                ValidateStorageIngestionTrackingAudit(maybeAudit, audit.Id);

                ValidateAgainstStorageIngestionTrackingAuditOnModify(
                    inputIngestionTrackingAudit: audit,
                    storageIngestionTrackingAudit: maybeAudit);

                return await this.storageBroker.UpdateIngestionTrackingAuditAsync(audit);
            });

        public ValueTask<IngestionTrackingAudit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                IngestionTrackingAudit maybeAudit = await this.storageBroker
                    .SelectIngestionTrackingAuditByIdAsync(auditId);

                ValidateStorageIngestionTrackingAudit(maybeAudit, auditId);

                var deletedItem = await this.storageBroker
                    .DeleteIngestionTrackingAuditAsync(maybeAudit);

                var confirmDeletedItem = await this.storageBroker
                    .SelectIngestionTrackingAuditByIdAsync(maybeAudit.Id);

                ValidateIngestionTrackingAuditIsNull(confirmDeletedItem);

                return deletedItem;
            });
    }
}