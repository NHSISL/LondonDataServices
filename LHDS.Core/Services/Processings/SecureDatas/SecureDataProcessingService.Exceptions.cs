// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingService
    {
        private delegate ValueTask<SubscriberCredential> ReturningSubscriberCredentialFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<SubscriberCredential> TryCatch(
            ReturningSubscriberCredentialFunction returningSubscriberCredentialFunction)
        {
            try
            {
                return await returningSubscriberCredentialFunction();
            }
            catch (InvalidSecureDataException invalidSecureDataException)
            {
                throw CreateAndLogValidationException(invalidSecureDataException);
            }
            catch (SecureDataDependencyValidationException secureDataDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(secureDataDependencyValidationException);
            }
            catch (SecureDataValidationException secureDataValidationException)
            {
                throw CreateAndLogDependencyValidationException(secureDataValidationException);
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialException);
            }
            catch (InvalidSubscriberCredentialException invalidSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(invalidSubscriberCredentialException);
            }
            catch (InvalidArgumentSubscriberCredentialProcessingException
                invalidArgumentSubscriberCredentialProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentSubscriberCredentialProcessingException);
            }
            catch (SecureDataDependencyException secureDataDependencyException)
            {
                throw CreateAndLogDependencyException(secureDataDependencyException);
            }
            catch (SecureDataServiceException secureDataServiceException)
            {
                throw CreateAndLogDependencyException(secureDataServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedSubscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberCredentialProcessingServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(nullSubscriberCredentialException);
            }
            catch (InvalidSubscriberCredentialException invalidSubscriberCredentialException)
            {
                throw CreateAndLogValidationException(invalidSubscriberCredentialException);
            }
            catch (InvalidArgumentSubscriberCredentialProcessingException
                invalidArgumentSubscriberCredentialProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentSubscriberCredentialProcessingException);
            }
            catch (SecureDataDependencyValidationException secureDataDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(secureDataDependencyValidationException);
            }
            catch (SecureDataValidationException secureDataValidationException)
            {
                throw CreateAndLogDependencyValidationException(secureDataValidationException);
            }
            catch (SecureDataDependencyException secureDataDependencyException)
            {
                throw CreateAndLogDependencyException(secureDataDependencyException);
            }
            catch (SecureDataServiceException secureDataServiceException)
            {
                throw CreateAndLogDependencyException(secureDataServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedSubscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberCredentialProcessingServiceException);
            }
        }

        private SubscriberCredentialValidationException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberCredentialValidationException = new SubscriberCredentialValidationException(
                message: "Subscriber credential validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialValidationException);

            return subscriberCredentialValidationException;
        }

        private SubscriberCredentialProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var subscriberCredentialProcessingDependencyValidationException =
                new SubscriberCredentialProcessingDependencyValidationException(
                    message: "Subscriber credential processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(subscriberCredentialProcessingDependencyValidationException);

            return subscriberCredentialProcessingDependencyValidationException;
        }

        private SubscriberCredentialProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var subscriberCredentialProcessingDependencyException =
                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(subscriberCredentialProcessingDependencyException);

            return subscriberCredentialProcessingDependencyException;
        }

        private SubscriberCredentialProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var subscriberCredentialProcessingServiceException = new
                SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberCredentialProcessingServiceException);

            return subscriberCredentialProcessingServiceException;
        }
    }
}