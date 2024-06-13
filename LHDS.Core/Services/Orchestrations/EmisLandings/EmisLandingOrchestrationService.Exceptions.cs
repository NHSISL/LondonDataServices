// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class EmisLandingOrchestrationService
    {
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<byte[]> ReturningByteArrayFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (InvalidArgumentEmisLandingOrchestrationException invalidArgumentEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException
                nullBlobContainersEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
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
                var failedDownloadServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentEmisLandingOrchestrationException invalidArgumentEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException nullBlobContainersEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (NotFoundEmisLandingOrchestrationException notFoundEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(notFoundEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedEmisLandingOrchestrationServiceException);
            }
        }

        private async ValueTask<byte[]> TryCatch(ReturningByteArrayFunction returningByteArrayFunction)
        {
            try
            {
                return await returningByteArrayFunction();
            }
            catch (InvalidArgumentEmisLandingOrchestrationException invalidArgumentEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException nullBlobContainersEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (NotFoundEmisLandingOrchestrationException notFoundEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(notFoundEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedEmisLandingOrchestrationServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentEmisLandingOrchestrationException invalidArgumentEmisLandingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDependencyException(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedEmisLandingOrchestrationServiceException);
            }
        }

        private EmisLandingOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var emisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    exception);

            this.loggingBroker.LogError(emisLandingOrchestrationValidationException);

            return emisLandingOrchestrationValidationException;
        }

        private EmisLandingOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var emisLandingOrchestrationDependencyValidationException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(emisLandingOrchestrationDependencyValidationException);

            return emisLandingOrchestrationDependencyValidationException;
        }

        private EmisLandingOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var emisLandingOrchestrationDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(emisLandingOrchestrationDependencyException);

            throw emisLandingOrchestrationDependencyException;
        }

        private EmisLandingOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var emisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    exception);

            this.loggingBroker.LogError(emisLandingOrchestrationServiceException);

            throw emisLandingOrchestrationServiceException;
        }
    }
}