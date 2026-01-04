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
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public OptOutService(
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
    }
}