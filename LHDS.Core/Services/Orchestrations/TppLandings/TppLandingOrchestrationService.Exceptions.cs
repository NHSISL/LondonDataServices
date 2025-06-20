// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppLandingOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullDocumentTppLandingException nullDocumentTppLandingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDocumentTppLandingException);
            }
            catch (InvalidArgumentTppLandingOrchestrationException invalidArgumentTppLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentTppLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(auditValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(auditDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(auditServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedTppLandingOrchestrationServiceException =
                    new FailedTppLandingOrchestrationServiceException(
                        message:
                            "Failed TPP landing orchestration aggregate service error occurred, " +
                            "please contact support.",
                        aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedTppLandingOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedTppLandingOrchestrationServiceException =
                    new FailedTppLandingOrchestrationServiceException(
                        message: "Failed TPP landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedTppLandingOrchestrationServiceException);
            }
        }

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (NullDocumentTppLandingException nullDocumentTppLandingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDocumentTppLandingException);
            }
            catch (InvalidArgumentTppLandingOrchestrationException invalidArgumentTppLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentTppLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(auditValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(auditDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingestionTrackingServiceException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(auditServiceException);
            }
            catch (Exception exception)
            {
                var failedTppLandingOrchestrationServiceException =
                    new FailedTppLandingOrchestrationServiceException(
                        message: "Failed TPP landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedTppLandingOrchestrationServiceException);
            }
        }

        private async ValueTask<TppLandingOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var tppLandingOrchestrationValidationException =
                new TppLandingOrchestrationValidationException(
                    message: "TPP landing orchestration validation errors occured, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(tppLandingOrchestrationValidationException);

            return tppLandingOrchestrationValidationException;
        }

        private async ValueTask<TppLandingOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var tppLandingOrchestrationDependencyValidationException =
                new TppLandingOrchestrationDependencyValidationException(
                    message: "TPP landing orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(tppLandingOrchestrationDependencyValidationException);

            return tppLandingOrchestrationDependencyValidationException;
        }

        private async ValueTask<TppLandingOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var tppLandingOrchestrationDependencyException =
                new TppLandingOrchestrationDependencyException(
                    message: "TPP landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(tppLandingOrchestrationDependencyException);

            throw tppLandingOrchestrationDependencyException;
        }

        private async ValueTask<TppLandingOrchestrationServiceException>
           CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var
                tppLandingOrchestrationServiceException =
                new TppLandingOrchestrationServiceException(
                    message: "TPP landing orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(tppLandingOrchestrationServiceException);

            throw tppLandingOrchestrationServiceException;
        }
    }
}
