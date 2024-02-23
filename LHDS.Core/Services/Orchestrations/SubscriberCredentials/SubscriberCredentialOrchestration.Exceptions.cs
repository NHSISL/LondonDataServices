// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
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
                    message: "Subscriber credential orchestration validation errors occurred, please try again.",
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
                    message: " orchestration service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialOrchestrationServiceException);

            throw subscriberCredentialOrchestrationServiceException;
        }
    }
}
