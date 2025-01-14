// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestration
    {
        private delegate ValueTask<SubscriberCredential> ReturningSubscriberCredentialFunction();
        private delegate ValueTask<IQueryable<SubscriberCredential>> ReturningSubscriberCredentialIQueryableFunction();
        private delegate ValueTask<List<Guid>> ReturningGuidListFunction();
        private delegate ValueTask ReturnNothingFunction();

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
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingServiceException);
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

        private async ValueTask<IQueryable<SubscriberCredential>>
            TryCatch(ReturningSubscriberCredentialIQueryableFunction returningSubscriberCredentialIQueryableFunction)
        {
            try
            {
                return await returningSubscriberCredentialIQueryableFunction();
            }
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingServiceException);
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

        private async ValueTask<List<Guid>> TryCatch(ReturningGuidListFunction ReturningGuidListFunction)
        {
            try
            {
                return await ReturningGuidListFunction();
            }
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingServiceException);
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

        private async ValueTask TryCatch(ReturnNothingFunction returnNothingFunction)
        {
            try
            {
                await returnNothingFunction();
            }
            catch (InvalidArgumentSubscriberCredentialOrchestrationException
                invalidArgumentSubscriberCredentialOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw CreateAndLogDependencyValidationException(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw CreateAndLogDependencyException(subscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, " +
                            "please contact support.",
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
