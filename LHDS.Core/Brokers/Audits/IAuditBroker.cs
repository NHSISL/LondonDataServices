// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Brokers.Audits
{
    public interface IAuditBroker
    {
        ValueTask<Audit> LogAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId,
            string? logLevel = "Information");

        ValueTask<Audit> LogInformationAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId);

        ValueTask<Audit> LogWarningAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId);

        ValueTask<Audit> LogErrorAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId);

        ValueTask<Audit> LogCriticalAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            Guid? correlationId);
    }
}
