// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Audits;

namespace LHDS.Core.Brokers.Audits
{
    public class AuditBroker : IAuditBroker
    {
        private readonly IAuditClient auditClient;

        public AuditBroker(IAuditClient auditClient) =>
            this.auditClient = auditClient;

        public async ValueTask<Audit> Log(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId,
            string? logLevel = "Information")
        {
            return await auditClient.LogAudit(auditType, title, message, fileName, correlationId, logLevel);
        }

        public async ValueTask<Audit> LogInformation(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId)
        {
            return await auditClient.LogAudit(auditType, title, message, fileName, correlationId, "Information");
        }

        public async ValueTask<Audit> LogWarning(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId)
        {
            return await auditClient.LogAudit(auditType, title, message, fileName, correlationId, "Warning");
        }

        public async ValueTask<Audit> LogError(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId)
        {
            return await auditClient.LogAudit(auditType, title, message, fileName, correlationId, "Error");
        }

        public async ValueTask<Audit> LogCritical(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId)
        {
            return await auditClient.LogAudit(auditType, title, message, fileName, correlationId, "Critical");
        }
    }
}
