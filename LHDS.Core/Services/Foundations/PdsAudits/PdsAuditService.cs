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
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public PdsAuditService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityAuditBroker securityAuditBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityAuditBroker = securityAuditBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PdsAudit> AddPdsAuditAsync(PdsAudit pdsAudit) =>
            TryCatch(async () =>
            {
                PdsAudit pdsAuditWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(pdsAudit);

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
                PdsAudit pdsAuditWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(pdsAudit);

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
    }
}