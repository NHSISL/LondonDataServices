// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;

namespace LHDS.Core.Services.Processings.IngestionTrackingAudits
{
    public interface IIngestionTrackingAuditProcessingService
    {
        ValueTask<IngestionTrackingAudit> AddIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        IQueryable<IngestionTrackingAudit> RetrieveAllIngestionTrackingAudits();
        ValueTask<IngestionTrackingAudit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId);
        ValueTask<IngestionTrackingAudit> RetrieveOrAddIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        ValueTask<IngestionTrackingAudit> ModifyOrAddIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        ValueTask<IngestionTrackingAudit> ModifyIngestionTrackingAuditAsync(IngestionTrackingAudit audit);
        ValueTask<IngestionTrackingAudit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId);
    }
}
