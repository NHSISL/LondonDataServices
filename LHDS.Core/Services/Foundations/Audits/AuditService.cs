using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService : IAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Audit> AddAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                ValidateAuditOnAdd(audit);

                return await this.storageBroker.InsertAuditAsync(audit);
            });

        public IQueryable<Audit> RetrieveAllAudits() =>
            TryCatch(() => this.storageBroker.SelectAllAudits());

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
                ValidateAuditOnModify(audit);

                Audit maybeAudit =
                    await this.storageBroker.SelectAuditByIdAsync(audit.Id);

                ValidateStorageAudit(maybeAudit, audit.Id);
                ValidateAgainstStorageAuditOnModify(inputAudit: audit, storageAudit: maybeAudit);

                return await this.storageBroker.UpdateAuditAsync(audit);
            });

        public ValueTask<Audit> RemoveAuditByIdAsync(Guid auditId) =>
            throw new NotImplementedException();
    }
}