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
        private delegate ValueTask<T> ReturningFunction<T>();

        private async ValueTask<T> TryCatch<T>(ReturningFunction<T> returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (InvalidArgumentDecryptionOrchestrationException invalidArgumentDecryptionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDecryptionOrchestrationException);
            }
            catch (NullBlobContainersDecryptionOrchestrationException
                nullBlobContainersDecryptionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersDecryptionOrchestrationException);
            }
            catch (NullSubscriberCredentialDecryptionOrchestrationException
                nullSubscriberCredentialDecryptionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialDecryptionOrchestrationException);
            }
            catch (NotFoundDecryptionOrchestrationException notFoundDecryptionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDecryptionOrchestrationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (CryptographyValidationException DecryptionValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(DecryptionValidationException);
            }
            catch (CryptographyDependencyValidationException DecryptionDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(DecryptionDependencyValidationException);
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
            catch (CryptographyDependencyException decryptionDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionDependencyException);
            }
            catch (CryptographyServiceException decryptionServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionServiceException);
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
                var failedDecryptServiceException =
                    new FailedDecryptionOrchestrationServiceException(
                        message: "Failed Decryption orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptServiceException);
            }
        }

        private async ValueTask<DecryptionOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private async ValueTask<DecryptionOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private async ValueTask<DecryptionOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decryptionOrchestrationDependencyException);

            return decryptionOrchestrationDependencyException;
        }

        private async ValueTask<DecryptionOrchestrationServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var decryptionServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}