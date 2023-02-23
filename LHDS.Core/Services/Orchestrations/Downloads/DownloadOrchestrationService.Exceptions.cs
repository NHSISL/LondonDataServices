// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationService
    {
        private delegate ValueTask ReturningProcesssFunction();

        private async ValueTask TryCatch(ReturningProcesssFunction returningProcessFunction)
        {
            try
            {
                await returningProcessFunction();
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(downloadDependencyValidationException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw CreateAndLogValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (AuditValidationException auditValidationException)
            {
                throw CreateAndLogValidationException(auditValidationException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
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
            catch (AuditDependencyException auditDependencyException)
            {
                throw CreateAndLogDependencyException(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                throw CreateAndLogDependencyException(auditServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private DownloadOrchestrationDependencyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);
            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var downloadOrchestrationServiceException =
                new DownloadOrchestrationServiceException(exception);

            this.loggingBroker.LogError(downloadOrchestrationServiceException);

            throw downloadOrchestrationServiceException;
        }
    }
}