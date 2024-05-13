// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Azure;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private delegate ValueTask<SecureData> ReturningSecureDataFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<SecureData> TryCatch(ReturningSecureDataFunction returningSecureDataFunction)
        {
            try
            {
                return await returningSecureDataFunction();
            }
            catch (NullSecureDataException nullSecureDataException)
            {
                throw CreateAndLogValidationException(nullSecureDataException);
            }
            catch (InvalidSecureDataException invalidSecureDataException)
            {
                throw CreateAndLogValidationException(invalidSecureDataException);
            }
            catch (InvalidArgumentSecureDataException invalidArgumentSecureDataException)
            {
                throw CreateAndLogValidationException(invalidArgumentSecureDataException);
            }
            catch (ArgumentException argumentException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: argumentException);

                throw CreateAndLogDependencyValidationException(failedSecureDataException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: requestFailedException);

                throw CreateAndLogDependencyException(failedSecureDataException);
            }
            catch (Exception exception)
            {
                var failedSecureDataServiceException = new FailedSecureDataServiceException(
                    message: "Failed secure data service error occurred, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedSecureDataServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentSecureDataException invalidArgumentSecureDataException)
            {
                throw CreateAndLogValidationException(invalidArgumentSecureDataException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: requestFailedException);

                throw CreateAndLogDependencyException(failedSecureDataException);
            }
            catch (Exception exception)
            {
                var failedSecureDataServiceException = new FailedSecureDataServiceException(
                    message: "Failed secure data service error occurred, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedSecureDataServiceException);
            }
        }

        private SecureDataValidationException CreateAndLogValidationException(Xeption exception)
        {
            var secureDataValidationException = new SecureDataValidationException(
                message: "Secure data validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataValidationException);

            return secureDataValidationException;
        }

        private SecureDataDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var secureDataDependencyValidationException = new SecureDataDependencyValidationException(
                message: "Secure data dependency validation errors occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataDependencyValidationException);

            return secureDataDependencyValidationException;
        }

        private SecureDataDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var secureDataDependencyException = new SecureDataDependencyException(
                message: "Secure data dependency errors occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataDependencyException);

            return secureDataDependencyException;
        }

        private SecureDataServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var secureDataServiceException = new SecureDataServiceException(
                message: "Secure data service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataServiceException);

            return secureDataServiceException;
        }
    }
}