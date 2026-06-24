// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<IngestionTrackingAudit> InsertIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<IngestionTrackingAudit>> SelectAllIngestionTrackingAuditsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTrackingAudit> SelectIngestionTrackingAuditByIdAsync(
            Guid ingestionTrackingAuditId,
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTrackingAudit> UpdateIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default);

        ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default);
    }
}
