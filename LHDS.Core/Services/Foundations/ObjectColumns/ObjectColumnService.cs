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
        private readonly ISecurityAuditBroker securityAuditBroker;
        private readonly ILoggingBroker loggingBroker;

        public ObjectColumnService(
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

        public ValueTask<ObjectColumn> AddObjectColumnAsync(ObjectColumn objectColumn) =>
            TryCatch(async () =>
            {
                ObjectColumn objectColumnWithAddAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(objectColumn);

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
                ObjectColumn objectColumnWithModifyAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(objectColumn);

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
                    await this.securityAuditBroker.ApplyRemoveAuditValuesAsync(maybeObjectColumn);

                ObjectColumn updatedObjectColumn =
                    await this.storageBroker.UpdateObjectColumnAsync(objectColumnWithDeleteAuditApplied);

                await ValidateAgainstStorageObjectColumnOnDeleteAsync(
                    objectColumn: updatedObjectColumn,
                    maybeObjectColumn: objectColumnWithDeleteAuditApplied);

                return await this.storageBroker.DeleteObjectColumnAsync(updatedObjectColumn);
            });
    }
}