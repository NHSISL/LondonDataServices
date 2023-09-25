// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using LHDS.Core.Models.Foundations.Decryptions.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
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
            catch (DocumentValidationException documentValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(documentDependencyValidationException);
            }
            catch (DecryptionValidationException DecryptionValidationException)
            {
                throw CreateAndLogDependencyValidationException(DecryptionValidationException);
            }
            catch (DecryptionDependencyValidationException DecryptionDependencyValidationException)
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
            catch (AuditValidationException auditValidationException)
            {
                throw CreateAndLogDependencyValidationException(auditValidationException);
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
            catch (DecryptionDependencyException decryptionDependencyException)
            {
                throw CreateAndLogDependencyException(decryptionDependencyException);
            }
            catch (DecryptionServiceException decryptionServiceException)
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
                var failedDecryptServiceException =
                    new FailedDecryptionOrchestrationServiceException(
                        message: "Failed Decryption orchestration service occurred, please contact support",
                        exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private DecryptionOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionOrchestrationValidationException =
                new DecryptionOrchestrationValidationException(
                    message: "Decryption orchestration validation errors occurred, please try again.",
                    exception);

            this.loggingBroker.LogError(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private DecryptionOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecryptionOrchestrationDependencyValidationException(
                    message: "Decryption orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private DecryptionOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var decryptionOrchestrationDependencyException =
                new DecryptionOrchestrationDependencyException(
                    message: "Decryption orchestration dependency error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionOrchestrationDependencyException);

            throw decryptionOrchestrationDependencyException;
        }

        private DecryptionOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var decryptionServiceException =
                new DecryptionOrchestrationServiceException(
                    message: "Decryption orchestration service error occurred, contact support.",
                    exception);

            this.loggingBroker.LogError(decryptionServiceException);

            return decryptionServiceException;
        }
    }
}