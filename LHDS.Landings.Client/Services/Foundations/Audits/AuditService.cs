using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Landings.Client.Brokers.DateTimes;
using LHDS.Landings.Client.Brokers.Loggings;
using LHDS.Landings.Client.Brokers.Storages;
using LHDS.Landings.Client.Models.Audits;

namespace LHDS.Landings.Client.Services.Foundations.Audits
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