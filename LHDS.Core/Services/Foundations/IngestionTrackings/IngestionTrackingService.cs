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
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService : IIngestionTrackingService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingService(
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

        public ValueTask<IngestionTracking> AddIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                IngestionTracking ingestionTrackingWithAddAuditApplied = await ApplyAddAuditAsync(ingestionTracking);
                await ValidateIngestionTrackingOnAddAsync(ingestionTrackingWithAddAuditApplied);

                return await this.storageBroker.InsertIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IQueryable<IngestionTracking>> RetrieveAllIngestionTrackingsAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllIngestionTrackingsAsync());

        public ValueTask<IngestionTracking> RetrieveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingId(ingestionTrackingId);

                IngestionTracking maybeIngestionTracking = await this.storageBroker
                    .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                return maybeIngestionTracking;
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByFileNameAsync(string fileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(fileName);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.storageBroker.SelectAllIngestionTrackingsAsync();

                IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                        .FirstOrDefault(ingestionTracking => ingestionTracking.FileName == fileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking?> RetrieveIngestionTrackingByEncryptedFileNameAsync(
            string encryptedFileName) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingFileName(encryptedFileName);

                IQueryable<IngestionTracking> allIngestionTrackings =
                    await this.storageBroker.SelectAllIngestionTrackingsAsync();

                IngestionTracking? maybeIngestionTracking = allIngestionTrackings
                        .FirstOrDefault(ingestionTracking => ingestionTracking.EncryptedFileName == encryptedFileName);

                return await ValueTask.FromResult(maybeIngestionTracking);
            });

        public ValueTask<IngestionTracking> ModifyIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            TryCatch(async () =>
            {
                IngestionTracking ingestionTrackingWithModifyAuditApplied = await ApplyModifyAuditAsync(ingestionTracking);
                await ValidateIngestionTrackingOnModifyAsync(ingestionTrackingWithModifyAuditApplied);

                IngestionTracking maybeIngestionTracking =
                    await this.storageBroker.SelectIngestionTrackingByIdAsync(ingestionTracking.Id);

                ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId: ingestionTracking.Id);

                ValidateAgainstStorageIngestionTrackingOnModify(
                    inputIngestionTracking: ingestionTracking,
                    storageIngestionTracking: maybeIngestionTracking);

                return await this.storageBroker.UpdateIngestionTrackingAsync(ingestionTracking);
            });

        public ValueTask<IngestionTracking> RemoveIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            TryCatch(async () =>
            {
                return await WithRetry(async () =>
                {
                    ValidateIngestionTrackingId(ingestionTrackingId: ingestionTrackingId);

                    IngestionTracking maybeIngestionTracking = await this.storageBroker
                        .SelectIngestionTrackingByIdAsync(ingestionTrackingId);

                    ValidateStorageIngestionTracking(maybeIngestionTracking, ingestionTrackingId);

                    IngestionTracking ingestionTrackingWithDeleteAuditApplied = 
                        await ApplyDeleteAuditAsync(maybeIngestionTracking);

                    IngestionTracking updatedIngestionTracking =
                        await this.storageBroker.UpdateIngestionTrackingAsync(ingestionTrackingWithDeleteAuditApplied);

                    await ValidateAgainstStorageIngestionTrackingOnDeleteAsync(
                        ingestionTracking: updatedIngestionTracking,
                        maybeIngestionTracking: ingestionTrackingWithDeleteAuditApplied);

                    return await this.storageBroker.DeleteIngestionTrackingAsync(updatedIngestionTracking);
                });
            });

        virtual internal async ValueTask<IngestionTracking> ApplyAddAuditAsync(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            ingestionTracking.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            ingestionTracking.CreatedDate = auditDateTimeOffset;
            ingestionTracking.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            ingestionTracking.UpdatedDate = auditDateTimeOffset;

            return ingestionTracking;
        }

        virtual internal async ValueTask<IngestionTracking> ApplyModifyAuditAsync(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            ingestionTracking.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            ingestionTracking.UpdatedDate = auditDateTimeOffset;

            return ingestionTracking;
        }

        virtual internal async ValueTask<IngestionTracking> ApplyDeleteAuditAsync(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            ingestionTracking.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            ingestionTracking.UpdatedDate = auditDateTimeOffset;
            
            return ingestionTracking;
        }
    }
}