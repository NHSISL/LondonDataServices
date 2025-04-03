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
using Org.BouncyCastle.Crypto;

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
                ObjectColumn objectColumnWithAddAuditApplied = await ApplyAddObjectColumnAsync(objectColumn);
                await ValidateOptOutOnAddAsync(objectColumnWithAddAuditApplied);

                return await this.storageBroker.InsertOptOutAsync(objectColumnWithAddAuditApplied);
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
                ObjectColumn objectColumnWithModifyAuditApplied = await ApplyModifyAuditAsync(objectColumn);
                await ValidateOptOutOnModifyAsync(objectColumnWithModifyAuditApplied);

                OptOut maybeOptOut =
                    await this.storageBroker.SelectOptOutByIdAsync(optOut.Id);

                ValidateStorageOptOut(maybeOptOut, optOut.Id);
                ValidateAgainstStorageOptOutOnModify(inputOptOut: optOut, storageOptOut: maybeOptOut);

                return await this.storageBroker.UpdateOptOutAsync(optOut);
            });

        public ValueTask<ObjectColumn> RemoveObjectColumnByIdAsync(Guid objectColumnId) =>
            TryCatch(async () =>
            {
                ValidateObjectColumnId(objectColumnId: objectColumnId);

                ObjectColumn maybeObjectColumn = await this.storageBroker
                    .SelectObjectColumnByIdAsync(objectColumnId);

                ValidateStorageObjectColumn(maybeObjectColumn, objectColumnId);

                ObjectColumn objectColumnWithDeleteAuditApplied =
                    await ApplyDeleteAuditAsync(maybeObjectColumn);

                ObjectColumn updatedObjectColumn =
                    await this.storageBroker.UpdateObjectColumnAsync(objectColumnWithDeleteAuditApplied);

                await ValidateAgainstStorageObjectColumnOnDeleteAsync(
                    objectColumn: updatedObjectColumn,
                    maybeObjectColumn: objectColumnWithDeleteAuditApplied);

                return await this.storageBroker.DeleteObjectColumnAsync(updatedObjectColumn);
            });

        virtual internal async ValueTask<ObjectColumn> ApplyAddObjectColumnAsync(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            objectColumn.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            objectColumn.CreatedDate = auditDateTimeOffset;
            objectColumn.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            objectColumn.UpdatedDate = auditDateTimeOffset;

            return objectColumn;
        }

        virtual internal async ValueTask<ObjectColumn> ApplyModifyAuditAsync(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            objectColumn.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            objectColumn.UpdatedDate = auditDateTimeOffset;

            return objectColumn;
        }

        virtual internal async ValueTask<ObjectColumn> ApplyDeleteAuditAsync(ObjectColumn objectColumn)
        {
            ValidateObjectColumnIsNotNull(objectColumn);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            objectColumn.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            objectColumn.UpdatedDate = auditDateTimeOffset;

            return objectColumn;
        }
    }
}