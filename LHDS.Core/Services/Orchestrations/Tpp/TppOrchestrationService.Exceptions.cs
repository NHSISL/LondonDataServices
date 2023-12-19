// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Foundations.Tpp.Exceptions;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService
    {
        private delegate ValueTask<Guid> ReturningGuidFunction();

        private async ValueTask<Guid> TryCatch(ReturningGuidFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (NullTppDocumentException nullTppDocumentException)
            {
                throw CreateAndLogValidationException(nullTppDocumentException);
            }
            catch (InvalidArgumentTppOrchestrationException invalidArgumentException)
            {
                throw CreateAndLogValidationException(invalidArgumentException);
            }
            catch (TppOrchestrationValidationException tppOrchestrationValidationException)
            {
                throw CreateAndLogValidationException(tppOrchestrationValidationException);
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
                var failedTppServiceException =
                    new FailedTppOrchestrationServiceException(
                        message: "Failed TPP orchestration service occurred, please contact support",
                        exception);

                throw CreateAndLogServiceException(failedTppServiceException);
            }
        }

        private TppOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tppDocumentValidationExceptionn =
                new TppOrchestrationValidationException(
                    message: "TPP Orchestration validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(tppDocumentValidationExceptionn);

            return tppDocumentValidationExceptionn;
        }

        private TppOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var tppOrchestrationDependencyValidationException =
                new TppOrchestrationDependencyValidationException(
                    message: "TPP Orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppOrchestrationDependencyValidationException);

            return tppOrchestrationDependencyValidationException;
        }

        private TppOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var tppOrchestrationDependencyException =
                new TppOrchestrationDependencyException(
                    message: "TPP Orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppOrchestrationDependencyException);

            throw tppOrchestrationDependencyException;
        }

        private TppOrchestrationServiceException
           CreateAndLogServiceException(Xeption exception)
        {
            var
                tppOrchestrationServiceException =
                new TppOrchestrationServiceException(
                    message: "TPP Orchestration service error occurred, contact support.",
                    exception);

            this.loggingBroker.LogError(tppOrchestrationServiceException);

            throw tppOrchestrationServiceException;
        }
    }
}
