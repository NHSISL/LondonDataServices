// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingService : ISubscriberAgreementProcessingService
    {
        private delegate ValueTask<SubscriberAgreement> ReturningSubscriberAgreementProcessingFunction();
        private delegate ValueTask<IQueryable<SubscriberAgreement>> ReturningSubscriberAgreementsFunction();

        private async ValueTask<SubscriberAgreement> TryCatch(
            ReturningSubscriberAgreementProcessingFunction returningSubscriberAgreementProcessingFunction)
        {
            try
            {
                return await returningSubscriberAgreementProcessingFunction();
            }
            catch (NullSubscriberAgreementProcessingException nullSubscriberAgreementException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberAgreementException);
            }
            catch (InvalidArgumentSubscriberAgreementProcessingException
                invalidArgumentSubscriberAgreementProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentSubscriberAgreementProcessingException);
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberAgreementValidationException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementProcessingServiceException =
                    new FailedSubscriberAgreementProcessingServiceException(
                        message: "Failed subscriber agreement processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberAgreementProcessingServiceException);
            }
        }

        private async ValueTask<IQueryable<SubscriberAgreement>> TryCatch(
            ReturningSubscriberAgreementsFunction returningSubscriberAgreementsFunction)
        {
            try
            {
                return await returningSubscriberAgreementsFunction();
            }
            catch (SubscriberAgreementValidationException subscriberAgreementValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(subscriberAgreementValidationException);
            }
            catch (SubscriberAgreementDependencyValidationException subscriberAgreementDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    subscriberAgreementDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException subscriberAgreementDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementDependencyException);
            }
            catch (SubscriberAgreementServiceException subscriberAgreementServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(subscriberAgreementServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementProcessingServiceException =
                    new FailedSubscriberAgreementProcessingServiceException(
                        message: "Failed subscriber agreement processing service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberAgreementProcessingServiceException);
            }
        }

        private async ValueTask<SubscriberAgreementProcessingValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var subscriberAgreementProcessingValidationException =
                new SubscriberAgreementProcessingValidationException(
                    message: "Subscriber agreement processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementProcessingValidationException);

            return subscriberAgreementProcessingValidationException;
        }

        private async ValueTask<SubscriberAgreementProcessingDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var dataSetProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(dataSetProcessingDependencyValidationException);

            return dataSetProcessingDependencyValidationException;
        }

        private async ValueTask<SubscriberAgreementProcessingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var subscriberAgreementProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementProcessingDependencyException);

            return subscriberAgreementProcessingDependencyException;
        }

        private async ValueTask<SubscriberAgreementProcessingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var subscriberAgreementProcessingServiceException = new
                SubscriberAgreementProcessingServiceException(
                    message: "Subscriber agreement processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementProcessingServiceException);

            return subscriberAgreementProcessingServiceException;
        }
    }
}
