// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingAuditProcessingService
    {
        private delegate ValueTask<Audit> ReturningIngestionTrackingAuditProcessingFunction();
        private delegate IQueryable<Audit> ReturningIngestionTrackingAuditsFunction();

        private async ValueTask<Audit> TryCatch(
            ReturningIngestionTrackingAuditProcessingFunction returningIngestionTrackingAuditProcessingFunction)
        {
            try
            {
                return await returningIngestionTrackingAuditProcessingFunction();
            }
            catch (NullIngestionTrackingAuditProcessingException nullIngestionTrackingAuditProcessingException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingAuditProcessingException);
            }
            catch (InvalidArgumentIngestionTrackingAuditProcessingException
                invalidArgumentIngestionTrackingAuditProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentIngestionTrackingAuditProcessingException);
            }
            catch (AuditValidationException ingestionTrackingAuditProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditProcessingValidationException);
            }
            catch (AuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditDependencyValidationException);
            }
            catch (AuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditDependencyException);
            }
            catch (AuditServiceException ingestionTrackingAuditServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingAuditProcessingServiceException);
            }
        }

        private IQueryable<Audit> TryCatch(ReturningIngestionTrackingAuditsFunction returningIngestionTrackingAuditsFunction)
        {
            try
            {
                return returningIngestionTrackingAuditsFunction();
            }
            catch (AuditValidationException ingestionTrackingAuditValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditValidationException);
            }
            catch (AuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditDependencyValidationException);
            }
            catch (AuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditDependencyException);
            }
            catch (AuditServiceException ingestionTrackingAuditServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingAuditProcessingServiceException);
            }
        }

        private IngestionTrackingAuditProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingAuditProcessingValidationExceptionn =
                new IngestionTrackingAuditProcessingValidationException(
                    message: "IngestionTrackingAudit processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingAuditProcessingValidationExceptionn);

            return ingestionTrackingAuditProcessingValidationExceptionn;
        }

        private IngestionTrackingAuditProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var ingestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(ingestionTrackingAuditProcessingDependencyValidationException);

            return ingestionTrackingAuditProcessingDependencyValidationException;
        }

        private IngestionTrackingAuditProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var ingestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(ingestionTrackingAuditProcessingDependencyException);

            throw ingestionTrackingAuditProcessingDependencyException;
        }

        private IngestionTrackingAuditProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var ingestionTrackingAuditProcessingServiceException = new
                IngestionTrackingAuditProcessingServiceException(
                    message: "IngestionTrackingAudit processing service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingAuditProcessingServiceException);

            return ingestionTrackingAuditProcessingServiceException;
        }
    }
}
