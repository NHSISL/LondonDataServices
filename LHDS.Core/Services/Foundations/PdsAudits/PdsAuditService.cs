// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.PdsAudits;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService : IPdsAuditService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public PdsAuditService(
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

        public ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit) =>
            TryCatch(async () =>
            {
                PdsAudit pdsAuditWithAddAuditApplied = await ApplyAddPdsAuditAsync(pdsAudit);
                await ValidatePdsAuditOnAddAsync(pdsAuditWithAddAuditApplied);

                return await this.storageBroker.InsertPdsAuditAsync(pdsAudit);
            });

        public ValueTask<IQueryable<PdsAudit>> RetrieveAllPdsAuditsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllPdsAuditsAsync());

        public ValueTask<PdsAudit> RetrievePdsAuditByIdAsync(Guid pdsAuditId) =>
            TryCatch(async () =>
            {
                ValidatePdsAuditId(pdsAuditId);

                PdsAudit maybePdsAudit = await this.storageBroker
                    .SelectPdsAuditByIdAsync(pdsAuditId);

                ValidateStoragePdsAudit(maybePdsAudit, pdsAuditId);

                return maybePdsAudit;
            });

        public ValueTask<PdsAudit> ModifyPdsAuditAsync(PdsAudit pdsAudit) =>
            TryCatch(async () =>
            {
                PdsAudit pdsAuditWithModifyAuditApplied = await ApplyModifyAuditAsync(pdsAudit);
                await ValidatePdsAuditOnModifyAsync(pdsAuditWithModifyAuditApplied);

                PdsAudit maybePdsAudit =
                    await this.storageBroker.SelectPdsAuditByIdAsync(pdsAudit.Id);

                ValidateStoragePdsAudit(maybePdsAudit, pdsAudit.Id);
                ValidateAgainstStoragePdsAuditOnModify(inputPdsAudit: pdsAudit, storagePdsAudit: maybePdsAudit);

                return await this.storageBroker.UpdatePdsAuditAsync(pdsAudit);
            });

        public ValueTask<PdsAudit> RemovePdsAuditByIdAsync(Guid pdsAuditId) =>
            TryCatch(async () =>
            {
                ValidatePdsAuditId(pdsAuditId);

                PdsAudit maybePdsAudit = await this.storageBroker
                    .SelectPdsAuditByIdAsync(pdsAuditId);

                ValidateStoragePdsAudit(maybePdsAudit, pdsAuditId);

                return await this.storageBroker.DeletePdsAuditAsync(maybePdsAudit);
            });

        virtual internal async ValueTask<PdsAudit> ApplyAddPdsAuditAsync(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            pdsAudit.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            pdsAudit.CreatedDate = auditDateTimeOffset;
            pdsAudit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            pdsAudit.UpdatedDate = auditDateTimeOffset;

            return pdsAudit;
        }

        virtual internal async ValueTask<PdsAudit> ApplyModifyAuditAsync(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            pdsAudit.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            pdsAudit.UpdatedDate = auditDateTimeOffset;

            return pdsAudit;
        }
    }
}