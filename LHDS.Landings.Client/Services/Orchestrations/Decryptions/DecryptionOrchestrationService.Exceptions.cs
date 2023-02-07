// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Landings.Client.Models.Orchestrations.Decryptions.Exceptions;
using Xeptions;

namespace LHDS.Landings.Client.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService
    {
        private delegate ValueTask ReturningDecryptFunction();

        private async ValueTask TryCatch(ReturningDecryptFunction returningDecryptFunction)
        {
            try
            {
                await returningDecryptFunction();
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDocumentValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDocumentDependencyValidationException(documentDependencyValidationException);
            }
            catch (DecryptionValidationException DecryptionValidationException)
            {
                throw CreateAndLogDecryptionValidationException(DecryptionValidationException);
            }
            catch (DecryptionDependencyValidationException DecryptionDependencyValidationException)
            {
                throw CreateAndLogDecryptionDependencyValidationException(DecryptionDependencyValidationException);
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
            catch (DecryptionDependencyException decryptionDependencyException)
            {
                throw CreateAndLogDownloadDependencyException(decryptionDependencyException);
            }
            catch (DecryptionServiceException decryptionServiceException)
            {
                throw CreateAndLogDownloadServiceException(decryptionServiceException);
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
                var failedDecryptServiceException =
                    new FailedDecryptionOrchestrationServiceException(exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private DecryptionOrchestrationServiceException CreateAndLogServiceException(
           Xeption exception)
        {
            var DecryptionServiceException =
                new DecryptionOrchestrationServiceException(exception);

            this.loggingBroker.LogError(DecryptionServiceException);

            return DecryptionServiceException;
        }

        private DecryptionOrchestrationDependencyValidationException CreateAndLogDocumentValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogDocumentDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);
            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogDecryptionValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogDecryptionDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogIngestionTrackingDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogIngestionTrackingValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogAuditValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogAuditDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }
        private DecryptionOrchestrationDependencyException
             CreateAndLogDocumentDependencyException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogDocumentServiceException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogDownloadDependencyException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogDownloadServiceException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
           CreateAndLogIngestionTrackingDependencyException(Xeption exception)
        {
            var documentOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(documentOrchestrationDependencyException);

            throw documentOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogIngestionTrackingServiceException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
          CreateAndLogAuditDependencyException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogAuditServiceException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }
    }
}