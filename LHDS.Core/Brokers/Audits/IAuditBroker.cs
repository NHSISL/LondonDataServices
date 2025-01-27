// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Brokers.Audits
{
    public interface IAuditBroker
    {
        ValueTask<Audit> Log(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId,
            string? logLevel = "Information");

        ValueTask<Audit> LogInformation(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId);

        ValueTask<Audit> LogWarning(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId);

        ValueTask<Audit> LogError(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId);

        ValueTask<Audit> LogCritical(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId);
    }
}
