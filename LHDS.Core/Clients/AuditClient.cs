// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Clients.AuditClient.Exceptions;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Services.Foundations.Audits;
using Xeptions;

namespace LHDS.Core.Clients
{
    public class AuditClient : IAuditClient
    {
        private readonly IAuditService auditService;

        public AuditClient(IAuditService auditService) =>
            this.auditService = auditService;

        public async ValueTask<Audit> LogAuditAsync(
            string auditType,
            string title,
            string? message,
            string? fileName,
            string? correlationId,
            string? logLevel = "Information")
        {
            try
            {
                return await this.auditService
                    .AddAuditAsync(auditType, title, message, fileName, correlationId, logLevel);
            }
            catch (AuditValidationException auditValidationException)
            {
                throw new AuditClientValidationException(
                    message: "Audit client validation error occurred, fix errors and try again.",
                    innerException: auditValidationException.InnerException as Xeption);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
            {
                throw new AuditClientValidationException(
                    message: "Audit client validation error occurred, fix errors and try again.",
                    innerException: auditDependencyValidationException.InnerException as Xeption);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                throw new AuditClientDependencyException(
                    message: "Audit client dependency error occurred, please contact support.",
                    innerException: auditDependencyException.InnerException as Xeption);
            }
            catch (AuditServiceException auditServiceException)
            {
                throw new AuditClientServiceException(
                    message: "Audit client service error occurred, fix errors and try again.",
                    auditServiceException.InnerException as Xeption);
            }
        }
    }
}
