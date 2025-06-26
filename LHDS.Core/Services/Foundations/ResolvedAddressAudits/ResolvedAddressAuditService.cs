using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;

namespace LHDS.Core.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditService : IResolvedAddressAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public ResolvedAddressAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ResolvedAddressAudit> AddResolvedAddressAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit) =>
                TryCatch(async () =>
                {
                    ResolvedAddressAudit resolvedAddressAuditWithAddAuditApplied =
                        await ApplyAddResolvedAddressAuditAsync(resolvedAddressAudit);

                    await ValidateResolvedAddressAuditOnAddAsync(resolvedAddressAuditWithAddAuditApplied);

                    return await this.storageBroker.InsertResolvedAddressAuditAsync(resolvedAddressAudit);
                });

        public ValueTask<IQueryable<ResolvedAddressAudit>> RetrieveAllResolvedAddressAuditsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllResolvedAddressAuditsAsync());

        public ValueTask<ResolvedAddressAudit> RetrieveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressAuditId(resolvedAddressAuditId);

                ResolvedAddressAudit maybeResolvedAddressAudit = await this.storageBroker
                    .SelectResolvedAddressAuditByIdAsync(resolvedAddressAuditId);

                ValidateStorageResolvedAddressAudit(maybeResolvedAddressAudit, resolvedAddressAuditId);

                return maybeResolvedAddressAudit;
            });

        public ValueTask<ResolvedAddressAudit> ModifyResolvedAddressAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit) =>
                TryCatch(async () =>
                {
                    ResolvedAddressAudit resolvedAddressAuditWithModifyAuditApplied = 
                        await ApplyModifyAuditAsync(resolvedAddressAudit);

                    await ValidateResolvedAddressAuditOnModifyAsync(resolvedAddressAuditWithModifyAuditApplied);

                    ResolvedAddressAudit maybeResolvedAddressAudit =
                        await this.storageBroker.SelectResolvedAddressAuditByIdAsync(resolvedAddressAudit.Id);

                    ValidateStorageResolvedAddressAudit(maybeResolvedAddressAudit, resolvedAddressAudit.Id);

                    ValidateAgainstStorageResolvedAddressAuditOnModify(inputResolvedAddressAudit: resolvedAddressAudit,
                        storageResolvedAddressAudit: maybeResolvedAddressAudit);

                    return await this.storageBroker.UpdateResolvedAddressAuditAsync(resolvedAddressAudit);
                });

        public ValueTask<ResolvedAddressAudit> RemoveResolvedAddressAuditByIdAsync(Guid resolvedAddressAuditId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressAuditId(resolvedAddressAuditId);

                ResolvedAddressAudit maybeResolvedAddressAudit = await this.storageBroker
                    .SelectResolvedAddressAuditByIdAsync(resolvedAddressAuditId);

                ValidateStorageResolvedAddressAudit(maybeResolvedAddressAudit, resolvedAddressAuditId);

                return await this.storageBroker.DeleteResolvedAddressAuditAsync(maybeResolvedAddressAudit);
            });

        virtual internal async ValueTask<ResolvedAddressAudit> ApplyAddResolvedAddressAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit)
        {
            ValidateResolvedAddressAuditIsNotNull(resolvedAddressAudit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            resolvedAddressAudit.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            resolvedAddressAudit.CreatedDate = auditDateTimeOffset;
            resolvedAddressAudit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            resolvedAddressAudit.UpdatedDate = auditDateTimeOffset;

            return resolvedAddressAudit;
        }

        virtual internal async ValueTask<ResolvedAddressAudit> ApplyModifyAuditAsync(
            ResolvedAddressAudit resolvedAddressAudit)
        {
            ValidateResolvedAddressAuditIsNotNull(resolvedAddressAudit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            resolvedAddressAudit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            resolvedAddressAudit.UpdatedDate = auditDateTimeOffset;

            return resolvedAddressAudit;
        }
    }
}
