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
using LHDS.Core.Models.Foundations.ObjectColumns;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService : IObjectColumnService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public ObjectColumnService(
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

        public ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn) =>
            TryCatch(async () =>
            {
                ObjectColumn objectColumnWithAddAuditApplied = await ApplyAddObjectColumnAsync(objectColumn);
                await ValidateObjectColumnOnAddAsync(objectColumnWithAddAuditApplied);

                return await this.storageBroker.InsertObjectColumnAsync(objectColumn);
            });

        public ValueTask<IQueryable<ObjectColumn>> RetrieveAllObjectColumnsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllObjectColumnsAsync());

        public ValueTask<ObjectColumn> RetrieveObjectColumnByIdAsync(Guid objectColumnId) =>
            TryCatch(async () =>
            {
                ValidateObjectColumnId(objectColumnId);

                ObjectColumn maybeObjectColumn = await this.storageBroker
                    .SelectObjectColumnByIdAsync(objectColumnId);

                ValidateStorageObjectColumn(maybeObjectColumn, objectColumnId);

                return maybeObjectColumn;
            });

        public ValueTask<ObjectColumn> ModifyObjectColumnAsync(ObjectColumn objectColumn) =>
            TryCatch(async () =>
            {
                ObjectColumn objectColumnWithModifyAuditApplied = await ApplyModifyAuditAsync(objectColumn);
                await ValidateObjectColumnOnModifyAsync(objectColumnWithModifyAuditApplied);

                ObjectColumn maybeObjectColumn =
                    await this.storageBroker.SelectObjectColumnByIdAsync(objectColumn.Id);

                ValidateStorageObjectColumn(maybeObjectColumn, objectColumn.Id);

                ValidateAgainstStorageObjectColumnOnModify(
                    inputObjectColumn: objectColumn,
                    storageObjectColumn: maybeObjectColumn);

                return await this.storageBroker.UpdateObjectColumnAsync(objectColumn);
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