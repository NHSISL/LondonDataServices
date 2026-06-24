// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTracking> IngestionTrackings { get; set; }

        public async ValueTask BulkInsertIngestionTrackingsAsync(
            List<IngestionTracking> ingestionTrackingItems,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkInsertAsync(ingestionTrackingItems, useTransaction, cancellationToken);

        public async ValueTask<IngestionTracking> InsertIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(ingestionTracking, cancellationToken);

        public async ValueTask<IQueryable<IngestionTracking>> SelectAllIngestionTrackingsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<IngestionTracking>(cancellationToken);

        public async ValueTask<IngestionTracking> SelectIngestionTrackingByIdAsync(
            Guid ingestionTrackingId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<IngestionTracking>(new object[] { ingestionTrackingId }, cancellationToken);

        public async ValueTask BulkUpdateIngestionTrackingsAsync(
            List<IngestionTracking> ingestionTrackingItems,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkUpdateAsync(ingestionTrackingItems, useTransaction, cancellationToken);

        public async ValueTask<IngestionTracking> UpdateIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(ingestionTracking, cancellationToken);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(ingestionTracking, cancellationToken);
    }
}
