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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException
                nullBlobContainersEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
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
                var failedDownloadServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedDownloadServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedDownloadServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException nullBlobContainersEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (NotFoundEmisLandingOrchestrationException notFoundEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingOrchestrationServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (NullLandingConfigurationEmisLandingOrchestrationException
                nullLandingConfigurationEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullLandingConfigurationEmisLandingOrchestrationException);
            }
            catch (NullSubscriberCredentialEmisLandingOrchestrationException
                nullSubscriberCredentialEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialEmisLandingOrchestrationException);
            }
            catch (NullBlobContainersEmisLandingOrchestrationException nullBlobContainersEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersEmisLandingOrchestrationException);
            }
            catch (NotFoundEmisLandingOrchestrationException notFoundEmisLandingOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingOrchestrationServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(downloadDependencyValidationException);
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
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(downloadServiceException);
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
                var failedEmisLandingOrchestrationServiceException =
                    new FailedEmisLandingOrchestrationServiceException(
                        message: "Failed EMIS landing orchestration service error occurred, please contact support.",
                        exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingOrchestrationServiceException);
            }
        }

        private async ValueTask<EmisLandingOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var emisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    exception);

            await this.loggingBroker.LogErrorAsync(emisLandingOrchestrationValidationException);

            return emisLandingOrchestrationValidationException;
        }

        private async ValueTask<EmisLandingOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var emisLandingOrchestrationDependencyValidationException =
                new EmisLandingOrchestrationDependencyValidationException(
                    message: "EMIS landing orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(emisLandingOrchestrationDependencyValidationException);

            return emisLandingOrchestrationDependencyValidationException;
        }

        private async ValueTask<EmisLandingOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var emisLandingOrchestrationDependencyException =
                new EmisLandingOrchestrationDependencyException(
                    message: "EMIS landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(emisLandingOrchestrationDependencyException);

            return emisLandingOrchestrationDependencyException;
        }

        private async ValueTask<EmisLandingOrchestrationServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var emisLandingOrchestrationServiceException =
                new EmisLandingOrchestrationServiceException(
                    message: "EMIS landing orchestration service error occurred, please contact support.",
                    exception);

            await this.loggingBroker.LogErrorAsync(emisLandingOrchestrationServiceException);

            return emisLandingOrchestrationServiceException;
        }
    }
}