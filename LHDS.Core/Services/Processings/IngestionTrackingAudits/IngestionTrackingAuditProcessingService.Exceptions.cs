// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingAuditProcessingService
    {
        private delegate ValueTask<IngestionTrackingAudit> ReturningIngestionTrackingAuditProcessingFunction();
        private delegate IQueryable<IngestionTrackingAudit> ReturningIngestionTrackingAuditsFunction();

        private async ValueTask<IngestionTrackingAudit> TryCatch(
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
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditProcessingValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingAuditProcessingServiceException);
            }
        }

        private IQueryable<IngestionTrackingAudit> TryCatch(ReturningIngestionTrackingAuditsFunction returningIngestionTrackingAuditsFunction)
        {
            try
            {
                return returningIngestionTrackingAuditsFunction();
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingAuditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, please contact support.",
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
                    message: "IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingAuditProcessingServiceException);

            return ingestionTrackingAuditProcessingServiceException;
        }
    }
}
