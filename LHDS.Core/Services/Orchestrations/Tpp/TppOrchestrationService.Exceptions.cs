// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
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
            catch (InvalidArgumentException invalidArgumentException)
            {
                throw CreateAndLogValidationException(invalidArgumentException);
            }
            catch (TppDocumentValidationException tppDocumentValidationException)
            {
                throw CreateAndLogValidationException(tppDocumentValidationException);
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
        }

        private TppDocumentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var tppDocumentValidationExceptionn =
                new TppDocumentValidationException(
                    message: "Tpp Document validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(tppDocumentValidationExceptionn);

            return tppDocumentValidationExceptionn;
        }

        private TppOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var tppOrchestrationDependencyValidationException =
                new TppOrchestrationDependencyValidationException(
                    message: "Tpp Orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(tppOrchestrationDependencyValidationException);

            return tppOrchestrationDependencyValidationException;
        }
    }
}
