// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Landings.Client.Models.Orchestrations.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Orchestrations.Downloads
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
                throw CreateAndLogDocumentValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDocumentDependencyValidationException(documentDependencyValidationException);
            }
            catch (DownloadValidationException downloadValidationException)
            {
                throw CreateAndLogDownloadValidationException(downloadValidationException);
            }
            catch (DownloadDependencyValidationException downloadDependencyValidationException)
            {
                throw CreateAndLogDownloadDependencyValidationException(downloadDependencyValidationException);
            }
            catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            {
                throw CreateAndLogIngestionTrackingValidationException(ingestionTrackingValidationException);
            }
            catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            {
                throw CreateAndLogIngestionTrackingDependencyValidationException(ingestionTrackingDependencyValidationException);
            }
            catch (AuditValidationException auditValidationException)
            {
                throw CreateAndLogAuditValidationException(auditValidationException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
            {
                throw CreateAndLogAuditDependencyValidationException(auditDependencyValidationException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw CreateAndLogDocumentDependencyException(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw CreateAndLogDocumentServiceException(documentServiceException);
            }
            catch (DownloadDependencyException downloadDependencyException)
            {
                throw CreateAndLogDownloadDependencyException(downloadDependencyException);
            }
            catch (DownloadServiceException downloadServiceException)
            {
                throw CreateAndLogDownloadServiceException(downloadServiceException);
            }
            catch (IngestionTrackingDependencyException ingestionTrackingDependencyException)
            {
                throw CreateAndLogIngestionTrackingDependencyException(ingestionTrackingDependencyException);
            }
            catch (IngestionTrackingServiceException ingestionTrackingServiceException)
            {
                throw CreateAndLogIngestionTrackingServiceException(ingestionTrackingServiceException);
            }
            catch (AuditDependencyException auditDependencyException)
            {
                throw CreateAndLogAuditDependencyException(auditDependencyException);
            }
            catch (AuditServiceException auditServiceException)
            {
                throw CreateAndLogAuditServiceException(auditServiceException);
            }
            catch (Exception exception)
            {
                var failedDownloadServiceException =
                    new FailedDownloadOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedDownloadServiceException);
            }
        }

        private DownloadOrchestrationServiceException CreateAndLogServiceException(
           Xeption exception)
        {
            var downloadServiceException =
                new DownloadOrchestrationServiceException(exception);

            this.loggingBroker.LogError(downloadServiceException);

            return downloadServiceException;
        }

        private DownloadOrchestrationDependencyValidationException CreateAndLogDocumentValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogDocumentDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);
            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogDownloadValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogDownloadDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogIngestionTrackingDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogIngestionTrackingValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogAuditValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyValidationException
            CreateAndLogAuditDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependencyValidationException =
                new DownloadOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependencyValidationException);

            return downloadOrchestrationDependencyValidationException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogDocumentDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogDocumentServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogDownloadDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogDownloadServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
           CreateAndLogIngestionTrackingDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogIngestionTrackingServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
          CreateAndLogAuditDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependencyException
            CreateAndLogAuditServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }
    }
}