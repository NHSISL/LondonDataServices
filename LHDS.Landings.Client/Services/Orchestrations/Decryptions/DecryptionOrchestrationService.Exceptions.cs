// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Landings.Client.Models.Audits.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions;
using LHDS.Landings.Client.Models.Foundations.Documents.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Landings.Client.Models.Orchestrations.Decryptions;
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
            //catch (IngestionTrackingValidationException ingestionTrackingValidationException)
            //{
            //    throw CreateAndLogIngestionTrackingValidationException(ingestionTrackingValidationException);
            //}
            //catch (IngestionTrackingDependencyValidationException ingestionTrackingDependencyValidationException)
            //{
            //    throw CreateAndLogIngestionTrackingDependencyValidationException(ingestionTrackingDependencyValidationException);
            //}
            catch (AuditValidationException auditValidationException)
            {
                throw CreateAndLogAuditValidationException(auditValidationException);
            }
            catch (AuditDependencyValidationException auditDependencyValidationException)
            {
                throw CreateAndLogAuditDependencyValidationException(auditDependencyValidationException);
            }
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
    }
}

