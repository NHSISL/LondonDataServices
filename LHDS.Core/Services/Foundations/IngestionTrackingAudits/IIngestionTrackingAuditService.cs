// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;

namespace LHDS.Core.Services.Foundations.IngestionTrackingAudits
{
    public interface IIngestionTrackingAuditService
    {
        ValueTask<IngestionTrackingAudit> AddIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        ValueTask<IQueryable<IngestionTrackingAudit>> RetrieveAllIngestionTrackingAuditsAsync();
        ValueTask<IngestionTrackingAudit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId);
        ValueTask<IngestionTrackingAudit> ModifyIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        ValueTask<IngestionTrackingAudit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId);
    }
}