// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestration
    {
        private delegate ValueTask<SubscriberCredential> ReturningSubscriberCredentialFunction();

        private async ValueTask<SubscriberCredential> TryCatch(ReturningSubscriberCredentialFunction
            returningSubscriberCredentialFunction)
        {
            try
            {
                return await returningSubscriberCredentialFunction();
            }
            catch (InvalidArgumentSubscriberCredentialOrchestrationException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (InvalidSubscriberCredentialOrchestrationException
                invalidSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidSubscriberCredentialOrchestrationException);
            }
            catch (InvalidSubscriberAgreementOrchestrationException
                invalidSubscriberAgreementOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidSubscriberAgreementOrchestrationException);
            }
            catch (SubscriberCredentialValidationException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogDependencyException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (SubscriberCredentialProcessingServiceException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogDependencyException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberCredentialOrchestrationServiceException);
            }
        }

        private SubscriberCredentialValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialValidationOrchestrationException);

            return subscriberCredentialValidationOrchestrationException;
        }

        private SubscriberCredentialOrchestrationDependencyValidationException
           CreateAndLogDependencyValidationException(Xeption exception)
        {
            var subscriberCredentialOrchestrationDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, fix the errors and try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(subscriberCredentialOrchestrationDependencyValidationException);

            return subscriberCredentialOrchestrationDependencyValidationException;
        }

        private SubscriberCredentialDependencyOrchestrationException
           CreateAndLogDependencyException(Xeption exception)
        {
            var subscriberCredentialDependencyOrchestrationException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(subscriberCredentialDependencyOrchestrationException);

            throw subscriberCredentialDependencyOrchestrationException;
        }

        private SubscriberCredentialOrchestrationServiceException
            CreateAndLogServiceException(Xeption exception)
        {
            var subscriberCredentialOrchestrationServiceException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialOrchestrationServiceException);

            throw subscriberCredentialOrchestrationServiceException;
        }
    }
}
