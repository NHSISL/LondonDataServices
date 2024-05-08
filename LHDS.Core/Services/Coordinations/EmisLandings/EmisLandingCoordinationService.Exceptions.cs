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
                throw CreateAndLogValidationException(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing aggregate coordination service error occurred, please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
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
                throw CreateAndLogValidationException(invalidArgumentEmisLandingCoordinationException);
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
            catch (EmisLandingOrchestrationValidationException
                emisLandingOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationValidationException);
            }
            catch (EmisLandingOrchestrationDependencyValidationException
                emisLandingOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(emisLandingOrchestrationDependencyValidationException);
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
            catch (EmisLandingOrchestrationDependencyException
                emisLandingOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationDependencyException);
            }
            catch (EmisLandingOrchestrationServiceException
                emisLandingOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(emisLandingOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedEmisLandingCoordinationServiceException =
                    new FailedEmisLandingCoordinationServiceException(
                        message: "Failed EMIS landing coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedEmisLandingCoordinationServiceException);
            }
        }

        private EmisLandingCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var emisLandingCoordinationValidationException =
                new EmisLandingCoordinationValidationException(
                    message: "Emis Landing coordination validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(emisLandingCoordinationValidationException);

            return emisLandingCoordinationValidationException;
        }

        private EmisLandingCoordinationDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var emisLandingCoordinationDependencyValidationException =
                new EmisLandingCoordinationDependencyValidationException(
                    message: "EMIS landing coordination dependency validation error occurred, please try again.",
                    exception.InnerException as Xeption);

            this.loggingBroker.LogError(emisLandingCoordinationDependencyValidationException);

            return emisLandingCoordinationDependencyValidationException;
        }

        private EmisLandingCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var emisLandingCoordinationDependencyException =
                new EmisLandingCoordinationDependencyException(
                    message: "EMIS landing coordination dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(emisLandingCoordinationDependencyException);

            return emisLandingCoordinationDependencyException;
        }

        private EmisLandingCoordinationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var emisLandingCoordinationServiceException =
                new EmisLandingCoordinationServiceException(
                    message: "EMIS landing coordination service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(emisLandingCoordinationServiceException);

            return emisLandingCoordinationServiceException;
        }
    }
}