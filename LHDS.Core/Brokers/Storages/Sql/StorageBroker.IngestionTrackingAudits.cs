// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTrackingAudit> IngestionTrackingAudits { get; set; }

        public async ValueTask<IngestionTrackingAudit> InsertIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(ingestionTrackingAudit, cancellationToken);

        public async ValueTask<IQueryable<IngestionTrackingAudit>> SelectAllIngestionTrackingAuditsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<IngestionTrackingAudit>(cancellationToken);

        public async ValueTask<IngestionTrackingAudit> SelectIngestionTrackingAuditByIdAsync(
            Guid ingestionTrackingAuditId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<IngestionTrackingAudit>(
                    new object[] { ingestionTrackingAuditId },
                    cancellationToken);

        public async ValueTask<IngestionTrackingAudit> UpdateIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(ingestionTrackingAudit, cancellationToken);

        public async ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(ingestionTrackingAudit, cancellationToken);
    }
}
