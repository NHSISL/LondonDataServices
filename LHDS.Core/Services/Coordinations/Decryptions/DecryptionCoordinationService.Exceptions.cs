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
                throw CreateAndLogValidationException(invalidArgumentDecryptionCoordinationException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialOrchestrationDependencyValidationException);
            }
            catch (DecryptionOrchestrationValidationException
                decryptionOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(decryptionOrchestrationValidationException);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(decryptionOrchestrationDependencyValidationException);
            }
            catch (IngressOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationValidationException);
            }
            catch (IngressOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationDependencyValidationException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialOrchestrationServiceException);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(decryptionOrchestrationDependencyException);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(decryptionOrchestrationServiceException);
            }
            catch (IngressOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationDependencyException);
            }
            catch (IngressOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptionCoordinationServiceException =
                    new FailedDecryptionCoordinationServiceException(
                        message: "Failed decryption coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptionCoordinationServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentDecryptionCoordinationException);
            }
            catch (SubscriberCredentialValidationOrchestrationException
                subscriberCredentialValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationDependencyValidationException
                subscriberCredentialOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialOrchestrationDependencyValidationException);
            }
            catch (DecryptionOrchestrationValidationException
                decryptionOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(decryptionOrchestrationValidationException);
            }
            catch (DecryptionOrchestrationDependencyValidationException
                decryptionOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(decryptionOrchestrationDependencyValidationException);
            }
            catch (IngressOrchestrationValidationException
                ingresOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationValidationException);
            }
            catch (IngressOrchestrationDependencyValidationException
                ingresOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(ingresOrchestrationDependencyValidationException);
            }
            catch (SubscriberCredentialDependencyOrchestrationException
                subscriberCredentialDependencyOrchestrationException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialDependencyOrchestrationException);
            }
            catch (SubscriberCredentialOrchestrationServiceException
                subscriberCredentialOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialOrchestrationServiceException);
            }
            catch (DecryptionOrchestrationDependencyException
                decryptionOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(decryptionOrchestrationDependencyException);
            }
            catch (DecryptionOrchestrationServiceException
                decryptionOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(decryptionOrchestrationServiceException);
            }
            catch (IngressOrchestrationDependencyException
                ingresOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationDependencyException);
            }
            catch (IngressOrchestrationServiceException
                ingresOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(ingresOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptionCoordinationServiceException =
                    new FailedDecryptionCoordinationServiceException(
                        message: "Failed decryption coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptionCoordinationServiceException);
            }
        }

        private DecryptionCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var decryptionCoordinationValidationException =
                new DecryptionCoordinationValidationException(
                    message: "Decryption coordination validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionCoordinationValidationException);

            return decryptionCoordinationValidationException;
        }

        private DecryptionCoordinationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var decryptionCoordinationDependencyValidationException =
                new DecryptionCoordinationDependencyValidationException(
                    message: "Decryption coordination dependency validation error occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionCoordinationDependencyValidationException);

            return decryptionCoordinationDependencyValidationException;
        }

        private DecryptionCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var decryptionCoordinationDependencyException =
                new DecryptionCoordinationDependencyException(
                    message: "Decryption coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(decryptionCoordinationDependencyException);

            return decryptionCoordinationDependencyException;
        }

        private DecryptionCoordinationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var decryptionCoordinationServiceException =
                new DecryptionCoordinationServiceException(
                    message: "Decryption coordination service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(decryptionCoordinationServiceException);

            return decryptionCoordinationServiceException;
        }
    }
}
