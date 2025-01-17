// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.EmisLandings.Exceptions;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.EmisLandings
{
    public partial class EmisLandingCoordinationService : IEmisLandingCoordinationService
    {
        private delegate ValueTask<List<string>> ReturningStringListFunction();
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<Document> ReturningDocumentFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
        }

        private async ValueTask<Document> TryCatch(ReturningDocumentFunction returningDocumentFunction)
        {
            try
            {
                return await returningDocumentFunction();
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentEmisLandingCoordinationException invalidArgumentEmisLandingCoordinationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(emisLandingOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedEmisLandingCoordinationServiceException);
            }
        }

        private async ValueTask<EmisLandingCoordinationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var emisLandingCoordinationValidationException =
                new EmisLandingCoordinationValidationException(
                    message: "Emis Landing coordination validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(emisLandingCoordinationValidationException);

            return emisLandingCoordinationValidationException;
        }

        private async ValueTask<EmisLandingCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var emisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(emisLandingCoordinationDependencyValidationException);

            return emisLandingCoordinationDependencyValidationException;
        }

        private async ValueTask<EmisLandingCoordinationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var emisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(emisLandingCoordinationDependencyException);

            return emisLandingCoordinationDependencyException;
        }

        private async ValueTask<EmisLandingCoordinationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var emisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(emisLandingCoordinationServiceException);

            return emisLandingCoordinationServiceException;
        }
    }
}