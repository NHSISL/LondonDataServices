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
                throw await CreateAndLogValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (InvalidSubscriberCredentialOrchestrationException
                invalidSubscriberCredentialOrchestrationException)
            {
                throw await CreateAndLogValidationException(invalidSubscriberCredentialOrchestrationException);
            }
            catch (InvalidSubscriberAgreementOrchestrationException
                invalidSubscriberAgreementOrchestrationException)
            {
                throw await CreateAndLogValidationException(invalidSubscriberAgreementOrchestrationException);
            }
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialOrchestrationServiceException);
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
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialOrchestrationServiceException);
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
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialOrchestrationServiceException);
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
                throw await CreateAndLogValidationException(invalidArgumentSubscriberCredentialOrchestrationException);
            }
            catch (SubscriberAgreementProcessingValidationException
                subscriberAgreementProcessingValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyValidationException
                subscriberAgreementProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementProcessingDependencyValidationException);
            }
            catch (SubscriberCredentialValidationException
                subscriberCredentialValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberCredentialValidationException);
            }
            catch (SubscriberCredentialProcessingDependencyValidationException
                subscriberCredentialProcessingDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberCredentialProcessingDependencyValidationException);
            }
            catch (SubscriberAgreementProcessingDependencyException
                subscriberAgreementProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingDependencyException);
            }
            catch (SubscriberAgreementProcessingServiceException
                subscriberAgreementProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementProcessingServiceException);
            }
            catch (SubscriberCredentialProcessingDependencyException
                subscriberCredentialProcessingDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingDependencyException);
            }
            catch (SubscriberCredentialProcessingServiceException
                subscriberCredentialProcessingServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialOrchestrationServiceException =
                    new FailedSubscriberCredentialOrchestrationServiceException(
                        message: "Failed subscriber credential orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialOrchestrationServiceException);
            }
        }

        private async ValueTask<SubscriberCredentialValidationOrchestrationException> 
            CreateAndLogValidationException(Xeption exception)
        {
            var subscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialValidationOrchestrationException);

            return subscriberCredentialValidationOrchestrationException;
        }

        private async ValueTask<SubscriberCredentialOrchestrationDependencyValidationException>
           CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var subscriberCredentialOrchestrationDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialOrchestrationDependencyValidationException);

            return subscriberCredentialOrchestrationDependencyValidationException;
        }

        private async ValueTask<SubscriberCredentialDependencyOrchestrationException>
           CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var subscriberCredentialDependencyOrchestrationException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialDependencyOrchestrationException);

            throw subscriberCredentialDependencyOrchestrationException;
        }

        private async ValueTask<SubscriberCredentialOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var subscriberCredentialOrchestrationServiceException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialOrchestrationServiceException);

            throw subscriberCredentialOrchestrationServiceException;
        }
    }
}
