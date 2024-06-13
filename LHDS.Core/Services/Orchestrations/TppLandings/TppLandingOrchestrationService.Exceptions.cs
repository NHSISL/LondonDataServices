// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (NullDocumentTppLandingException nullDocumentTppLandingException)
            {
                throw CreateAndLogValidationException(nullDocumentTppLandingException);
            }
            catch (InvalidArgumentTppLandingOrchestrationException invalidArgumentTppLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentTppLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (IngestionTrackingAuditValidationException auditValidationException)
            {
                throw CreateAndLogDependencyValidationException(auditValidationException);
            }
            catch (IngestionTrackingAuditDependencyValidationException auditDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(auditDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDependencyException(documentServiceException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw CreateAndLogDependencyException(ingestionTrackingServiceException);
            }
            catch (IngestionTrackingAuditDependencyException auditDependencyException)
            {
                throw CreateAndLogDependencyException(auditDependencyException);
            }
            catch (IngestionTrackingAuditServiceException auditServiceException)
            {
                throw CreateAndLogDependencyException(auditServiceException);
            }
            catch (Exception exception)
            {
                var failedTppLandingOrchestrationServiceException =
                    new FailedTppLandingOrchestrationServiceException(
                        message: "Failed TPP landing orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedTppLandingOrchestrationServiceException);
            }
        }

        private TppLandingOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tppLandingOrchestrationValidationException =
                new TppLandingOrchestrationValidationException(
                    message: "TPP landing orchestration validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(tppLandingOrchestrationValidationException);

            return tppLandingOrchestrationValidationException;
        }

        private TppLandingOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var tppLandingOrchestrationDependencyValidationException =
                new TppLandingOrchestrationDependencyValidationException(
                    message: "TPP landing orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppLandingOrchestrationDependencyValidationException);

            return tppLandingOrchestrationDependencyValidationException;
        }

        private TppLandingOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var tppLandingOrchestrationDependencyException =
                new TppLandingOrchestrationDependencyException(
                    message: "TPP landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppLandingOrchestrationDependencyException);

            throw tppLandingOrchestrationDependencyException;
        }

        private TppLandingOrchestrationServiceException
           CreateAndLogServiceException(Xeption exception)
        {
            var
                tppLandingOrchestrationServiceException =
                new TppLandingOrchestrationServiceException(
                    message: "TPP landing orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(tppLandingOrchestrationServiceException);

            throw tppLandingOrchestrationServiceException;
        }
    }
}
