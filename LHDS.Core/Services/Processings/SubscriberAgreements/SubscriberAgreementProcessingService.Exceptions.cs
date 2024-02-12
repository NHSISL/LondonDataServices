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
        private delegate IQueryable<SubscriberAgreement> ReturningSubscriberAgreementsFunction();

        private async ValueTask<SubscriberAgreement> TryCatch(
            ReturningSubscriberAgreementProcessingFunction returningSubscriberAgreementProcessingFunction)
        {
            try
            {
                return await returningSubscriberAgreementProcessingFunction();
            }
            catch (NullSubscriberAgreementProcessingException nullSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(nullSubscriberAgreementException);
            }
            catch (InvalidArgumentSubscriberAgreementProcessingException
                invalidArgumentSubscriberAgreementProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentSubscriberAgreementProcessingException);
            }
            catch (SubscriberAgreementValidationException dataSetValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetValidationException);
            }
            catch (SubscriberAgreementDependencyValidationException dataSetDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException dataSetDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetDependencyException);
            }
            catch (SubscriberAgreementServiceException dataSetServiceException)
            {
                throw CreateAndLogDependencyException(dataSetServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementProcessingServiceException =
                    new FailedSubscriberAgreementProcessingServiceException(
                        message: "Failed subscriber agreement processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberAgreementProcessingServiceException);
            }
        }

        private IQueryable<SubscriberAgreement> TryCatch(
            ReturningSubscriberAgreementsFunction returningSubscriberAgreementsFunction)
        {
            try
            {
                return returningSubscriberAgreementsFunction();
            }
            catch (SubscriberAgreementValidationException dataSetValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetValidationException);
            }
            catch (SubscriberAgreementDependencyValidationException dataSetDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(dataSetDependencyValidationException);
            }
            catch (SubscriberAgreementDependencyException dataSetDependencyException)
            {
                throw CreateAndLogDependencyException(dataSetDependencyException);
            }
            catch (SubscriberAgreementServiceException dataSetServiceException)
            {
                throw CreateAndLogDependencyException(dataSetServiceException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementProcessingServiceException =
                    new FailedSubscriberAgreementProcessingServiceException(
                        message: "Failed subscriber agreement processing service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberAgreementProcessingServiceException);
            }
        }


        private SubscriberAgreementProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var dataSetProcessingValidationExceptionn =
                new SubscriberAgreementProcessingValidationException(
                    message: "Subscriber agreement processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetProcessingValidationExceptionn);

            return dataSetProcessingValidationExceptionn;
        }

        private SubscriberAgreementProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var dataSetProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetProcessingDependencyValidationException);

            return dataSetProcessingDependencyValidationException;
        }

        private SubscriberAgreementProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var dataSetProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(dataSetProcessingDependencyException);

            throw dataSetProcessingDependencyException;
        }

        private SubscriberAgreementProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var dataSetProcessingServiceException = new
                SubscriberAgreementProcessingServiceException(
                    message: "Subscriber agreement processing service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(dataSetProcessingServiceException);

            return dataSetProcessingServiceException;
        }
    }
}
