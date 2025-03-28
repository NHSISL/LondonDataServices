// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.Decryptions.Exceptions;
using LHDS.Core.Models.Orchestrations.Decryptions.Exceptions;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.Decryptions
{
    public partial class DecryptionCoordinationService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentDecryptionCoordinationException invalidArgumentDecryptionCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDecryptionCoordinationException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialOrchestrationDependencyValidationException);
            }
            catch (DecryptionOrchestrationValidationException
                decryptionOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decryptionOrchestrationValidationException);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decryptionOrchestrationDependencyValidationException);
            }
            catch (IngressOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationValidationException);
            }
            catch (IngressOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationDependencyValidationException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialOrchestrationServiceException);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionOrchestrationDependencyException);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionOrchestrationServiceException);
            }
            catch (IngressOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationDependencyException);
            }
            catch (IngressOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptionCoordinationServiceException =
                    new FailedDecryptionCoordinationServiceException(
                        message: "Failed decryption coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptionCoordinationServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentDecryptionCoordinationException invalidArgumentDecryptionCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentDecryptionCoordinationException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialOrchestrationDependencyValidationException);
            }
            catch (DecryptionOrchestrationValidationException
                decryptionOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decryptionOrchestrationValidationException);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decryptionOrchestrationDependencyValidationException);
            }
            catch (IngressOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationValidationException);
            }
            catch (IngressOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(ingresOrchestrationDependencyValidationException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialOrchestrationServiceException);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionOrchestrationDependencyException);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decryptionOrchestrationServiceException);
            }
            catch (IngressOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationDependencyException);
            }
            catch (IngressOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptionCoordinationServiceException =
                    new FailedDecryptionCoordinationServiceException(
                        message: "Failed decryption coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptionCoordinationServiceException);
            }
        }

        private async ValueTask<DecryptionCoordinationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var decryptionCoordinationValidationException =
                new DecryptionCoordinationValidationException(
                    message: "Decryption coordination validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionCoordinationValidationException);

            return decryptionCoordinationValidationException;
        }

        private async ValueTask<DecryptionCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var decryptionCoordinationDependencyValidationException =
                new DecryptionCoordinationDependencyValidationException(
                    message: "Decryption coordination dependency validation error occurred, please try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decryptionCoordinationDependencyValidationException);

            return decryptionCoordinationDependencyValidationException;
        }

        private async ValueTask<DecryptionCoordinationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var decryptionCoordinationDependencyException =
                new DecryptionCoordinationDependencyException(
                    message: "Decryption coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decryptionCoordinationDependencyException);

            return decryptionCoordinationDependencyException;
        }

        private async ValueTask<DecryptionCoordinationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var decryptionCoordinationServiceException =
                new DecryptionCoordinationServiceException(
                    message: "Decryption coordination service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decryptionCoordinationServiceException);

            return decryptionCoordinationServiceException;
        }
    }
}
