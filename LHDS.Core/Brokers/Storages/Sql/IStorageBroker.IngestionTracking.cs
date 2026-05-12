// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackings;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask BulkInsertIngestionTrackingsAsync(
            List<IngestionTracking> ingestionTrackingItems,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTracking> InsertIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<IngestionTracking>> SelectAllIngestionTrackingsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTracking> SelectIngestionTrackingByIdAsync(
            Guid ingestionTrackingId,
            CancellationToken cancellationToken = default);

        ValueTask BulkUpdateIngestionTrackingsAsync(
            List<IngestionTracking> ingestionTrackingItems,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTracking> UpdateIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTracking> DeleteIngestionTrackingAsync(
            IngestionTracking ingestionTracking,
            CancellationToken cancellationToken = default);
    }
}