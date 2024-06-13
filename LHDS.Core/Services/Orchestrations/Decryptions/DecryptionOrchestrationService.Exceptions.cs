// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Cryptographies.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Decryptions
{
    public partial class DecryptionOrchestrationService
    {
        private delegate ValueTask<string> ReturningDecryptFunction();

        private async ValueTask<string> TryCatch(ReturningDecryptFunction returningDecryptFunction)
        {
            try
            {
                return await returningDecryptFunction();
            }
            catch (InvalidArgumentDecryptionOrchestrationException invalidArgumentDecryptionOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentDecryptionOrchestrationException);
            }
            catch (NullBlobContainersDecryptionOrchestrationException
                nullBlobContainersDecryptionOrchestrationException)
            {
                throw CreateAndLogValidationException(nullBlobContainersDecryptionOrchestrationException);
            }
            catch (NullSubscriberCredentialDecryptionOrchestrationException
                nullSubscriberCredentialDecryptionOrchestrationException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialDecryptionOrchestrationException);
            }
            catch (NotFoundDecryptionOrchestrationException notFoundDecryptionOrchestrationException)
            {
                throw CreateAndLogValidationException(notFoundDecryptionOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (CryptographyValidationException DecryptionValidationException)
            {
                throw CreateAndLogDependencyValidationException(DecryptionValidationException);
            }
            catch (CryptographyDependencyValidationException DecryptionDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(DecryptionDependencyValidationException);
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
            catch (CryptographyDependencyException decryptionDependencyException)
            {
                throw CreateAndLogDependencyException(decryptionDependencyException);
            }
            catch (CryptographyServiceException decryptionServiceException)
            {
                throw CreateAndLogDependencyException(decryptionServiceException);
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
                var failedDecryptServiceException =
                    new FailedDecryptionOrchestrationServiceException(
                        message: "Failed Decryption orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private DecryptionOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var decryptionServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}