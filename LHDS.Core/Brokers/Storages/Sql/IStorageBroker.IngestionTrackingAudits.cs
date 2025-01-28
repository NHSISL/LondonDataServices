// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<IngestionTrackingAudit> InsertIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit);

        ValueTask<IQueryable<IngestionTrackingAudit>> SelectAllIngestionTrackingAuditsAsync();
        ValueTask<IngestionTrackingAudit> SelectIngestionTrackingAuditByIdAsync(Guid ingestionTrackingAuditId);

        ValueTask<IngestionTrackingAudit> UpdateIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit);

        ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit);
    }
}
