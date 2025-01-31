// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        private delegate ValueTask<IQueryable<IngestionTrackingAudit>> ReturningIngestionTrackingAuditsFunction();

        private async ValueTask<IngestionTrackingAudit> TryCatch(
            ReturningIngestionTrackingAuditProcessingFunction returningIngestionTrackingAuditProcessingFunction)
        {
            try
            {
                return await returningIngestionTrackingAuditProcessingFunction();
            }
            catch (NullIngestionTrackingAuditProcessingException nullIngestionTrackingAuditProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingAuditProcessingException);
            }
            catch (InvalidArgumentIngestionTrackingAuditProcessingException
                invalidArgumentIngestionTrackingAuditProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(
                    invalidArgumentIngestionTrackingAuditProcessingException);
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingAuditProcessingValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingAuditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingAuditProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<IngestionTrackingAudit>> TryCatch(
            ReturningIngestionTrackingAuditsFunction returningIngestionTrackingAuditsFunction)
        {
            try
            {
                return await returningIngestionTrackingAuditsFunction();
            }
            catch (IngestionTrackingAuditValidationException ingestionTrackingAuditValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingAuditValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException
                ingestionTrackingAuditDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    ingestionTrackingAuditDependencyValidationException);
            }
            catch (IngestionTrackingAuditDependencyException ingestionTrackingAuditDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingAuditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException ingestionTrackingAuditServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingAuditServiceException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditProcessingServiceException =
                    new FailedIngestionTrackingAuditProcessingServiceException(
                        message: "Failed IngestionTrackingAudit processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingAuditProcessingServiceException);
            }
        }

        private async ValueTask<IngestionTrackingAuditProcessingValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingAuditProcessingValidationExceptionn =
                new IngestionTrackingAuditProcessingValidationException(
                    message: "IngestionTrackingAudit processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingAuditProcessingValidationExceptionn);

            return ingestionTrackingAuditProcessingValidationExceptionn;
        }

        private async ValueTask<IngestionTrackingAuditProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, " +
                        "please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingAuditProcessingDependencyValidationException);

            return ingestionTrackingAuditProcessingDependencyValidationException;
        }

        private async ValueTask<IngestionTrackingAuditProcessingDependencyException> 
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var ingestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingAuditProcessingDependencyException);

            throw ingestionTrackingAuditProcessingDependencyException;
        }

        private async ValueTask<IngestionTrackingAuditProcessingServiceException> 
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var ingestionTrackingAuditProcessingServiceException = new
                IngestionTrackingAuditProcessingServiceException(
                    message: "IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingAuditProcessingServiceException);

            return ingestionTrackingAuditProcessingServiceException;
        }
    }
}
