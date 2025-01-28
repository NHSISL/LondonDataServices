// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Clients
{
    public interface IAuditClient
    {
        ValueTask<Audit> LogAuditAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId,
            string? logLevel = "Information");
    }
}
