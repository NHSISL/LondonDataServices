// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public interface IIngestionTrackingAuditProcessingService
    {
        ValueTask<Audit> AddIngestionTrackingAuditAsync(Audit audit);
        IQueryable<Audit> RetrieveAllIngestionTrackingAudits();
        ValueTask<Audit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId);
        ValueTask<Audit> RetrieveOrAddIngestionTrackingAuditAsync(Audit audit);
        ValueTask<Audit> ModifyOrAddIngestionTrackingAuditAsync(Audit audit);
        ValueTask<Audit> ModifyIngestionTrackingAuditAsync(Audit audit);
        ValueTask<Audit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId);
    }
}
