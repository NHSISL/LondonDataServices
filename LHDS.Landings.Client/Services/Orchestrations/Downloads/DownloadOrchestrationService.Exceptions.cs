// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

    }
}
