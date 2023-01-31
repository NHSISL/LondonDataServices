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
                var failedBoroughServiceException =
                    new FailedDownloadOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedBoroughServiceException);
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

        private DownloadOrchestrationDependancyValidationException CreateAndLogDocumentValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogDocumentDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);
            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogDownloadValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogDownloadDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogIngestionTrackingDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogIngestionTrackingValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogAuditValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyValidationException
            CreateAndLogAuditDependencyValidationException(Xeption exception)
        {
            var downloadOrchestrationDependancyValidationException =
                new DownloadOrchestrationDependancyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(downloadOrchestrationDependancyValidationException);

            return downloadOrchestrationDependancyValidationException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogDocumentDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogDocumentServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogDownloadDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogDownloadServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
           CreateAndLogIngestionTrackingDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogIngestionTrackingServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
          CreateAndLogAuditDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DownloadOrchestrationDependancyException
            CreateAndLogAuditServiceException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DownloadOrchestrationDependancyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }
    }
}
