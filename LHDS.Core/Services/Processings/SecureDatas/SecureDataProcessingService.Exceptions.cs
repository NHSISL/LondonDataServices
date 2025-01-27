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
                throw await CreateAndLogValidationExceptionAsync(invalidSecureDataException);
            }
            catch (SecureDataDependencyValidationException secureDataDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(secureDataDependencyValidationException);
            }
            catch (SecureDataValidationException secureDataValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(secureDataValidationException);
            }
            catch (NullSubscriberCredentialException nullSubscriberCredentialException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialException);
            }
            catch (InvalidSubscriberCredentialException invalidSubscriberCredentialException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSubscriberCredentialException);
            }
            catch (InvalidArgumentSubscriberCredentialProcessingException
                invalidArgumentSubscriberCredentialProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(
                    invalidArgumentSubscriberCredentialProcessingException);
            }
            catch (SecureDataDependencyException secureDataDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(secureDataDependencyException);
            }
            catch (SecureDataServiceException secureDataServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(secureDataServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message:
                            "Failed subscriber credential processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialProcessingServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberCredentialException);
            }
            catch (InvalidSubscriberCredentialException invalidSubscriberCredentialException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSubscriberCredentialException);
            }
            catch (InvalidArgumentSubscriberCredentialProcessingException
                invalidArgumentSubscriberCredentialProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentSubscriberCredentialProcessingException);
            }
            catch (SecureDataDependencyValidationException secureDataDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(secureDataDependencyValidationException);
            }
            catch (SecureDataValidationException secureDataValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(secureDataValidationException);
            }
            catch (SecureDataDependencyException secureDataDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(secureDataDependencyException);
            }
            catch (SecureDataServiceException secureDataServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(secureDataServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message: "Failed subscriber credential aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberCredentialProcessingServiceException =
                    new FailedSubscriberCredentialProcessingServiceException(
                        message:
                            "Failed subscriber credential processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberCredentialProcessingServiceException);
            }
        }

        private async ValueTask<SubscriberCredentialValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var subscriberCredentialValidationException = new SubscriberCredentialValidationException(
                message: "Subscriber credential validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialValidationException);

            return subscriberCredentialValidationException;
        }

        private async ValueTask<SubscriberCredentialProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var subscriberCredentialProcessingDependencyValidationException =
                new SubscriberCredentialProcessingDependencyValidationException(
                    message:
                        "Subscriber credential processing dependency validation error occurred, " +
                        "please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialProcessingDependencyValidationException);

            return subscriberCredentialProcessingDependencyValidationException;
        }

        private async ValueTask<SubscriberCredentialProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var subscriberCredentialProcessingDependencyException =
                new SubscriberCredentialProcessingDependencyException(
                    message: "Subscriber credential processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialProcessingDependencyException);

            return subscriberCredentialProcessingDependencyException;
        }

        private async ValueTask<SubscriberCredentialProcessingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var subscriberCredentialProcessingServiceException = new
                SubscriberCredentialProcessingServiceException(
                    message: "Subscriber credential processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberCredentialProcessingServiceException);

            return subscriberCredentialProcessingServiceException;
        }
    }
}