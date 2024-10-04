// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<IngestionTrackingAudit> IngestionTrackingAudits { get; set; }

        public async ValueTask<IngestionTrackingAudit> InsertIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit) =>
                await InsertAsync(ingestionTrackingAudit);

        public IQueryable<IngestionTrackingAudit> SelectAllIngestionTrackingAudits() =>
            SelectAll<IngestionTrackingAudit>();

        public async ValueTask<IngestionTrackingAudit> SelectIngestionTrackingAuditByIdAsync(
            Guid ingestionTrackingAuditId) =>
                await SelectAsync<IngestionTrackingAudit>(ingestionTrackingAuditId);

        public async ValueTask<IngestionTrackingAudit> UpdateIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit) =>
                await UpdateAsync(ingestionTrackingAudit);

        public async ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit) =>
                await DeleteAsync(ingestionTrackingAudit);
    }
}
