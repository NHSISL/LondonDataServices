// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Services.Foundations.Audits
{
    public interface IAuditService
    {
        ValueTask<Audit> AddAudit(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId,
            string? logLevel = "Information");

        ValueTask<Audit> AddAuditAsync(Audit audit);
        IQueryable<Audit> RetrieveAllAudits();
        ValueTask<Audit> RetrieveAuditByIdAsync(Guid auditId);
        ValueTask<Audit> ModifyAuditAsync(Audit audit);
        ValueTask<Audit> RemoveAuditByIdAsync(Guid auditId);

    }
}