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
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService : IOptOutService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public OptOutService(
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

        public ValueTask<OptOut> AddOptOutAsync(OptOut optOut) =>
            TryCatch(async () =>
            {
                OptOut optOutWithAddAuditApplied = await ApplyAddOptOutAsync(optOut);
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
                OptOut optOutWithModifyAuditApplied = await ApplyModifyAuditAsync(optOut);
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

        virtual internal async ValueTask<OptOut> ApplyAddOptOutAsync(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            optOut.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            optOut.CreatedDate = auditDateTimeOffset;
            optOut.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            optOut.UpdatedDate = auditDateTimeOffset;

            return optOut;
        }

        virtual internal async ValueTask<OptOut> ApplyModifyAuditAsync(OptOut optOut)
        {
            ValidateOptOutIsNotNull(optOut);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            optOut.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            optOut.UpdatedDate = auditDateTimeOffset;

            return optOut;
        }
    }
}