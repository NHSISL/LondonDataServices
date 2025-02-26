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
                throw await CreateAndLogValidationExceptionAsync(nullSecureDataException);
            }
            catch (InvalidSecureDataException invalidSecureDataException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSecureDataException);
            }
            catch (InvalidArgumentSecureDataException invalidArgumentSecureDataException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentSecureDataException);
            }
            catch (ArgumentException argumentException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(failedSecureDataException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: requestFailedException);

                throw await CreateAndLogDependencyExceptionAsync(failedSecureDataException);
            }
            catch (Exception exception)
            {
                var failedSecureDataServiceException = new FailedSecureDataServiceException(
                    message: "Failed secure data service error occurred, please contact support.",
                    innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSecureDataServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentSecureDataException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedSecureDataException =
                    new FailedSecureDataException(
                        message: "Failed secure data error occurred, please contact support.",
                        innerException: requestFailedException);

                throw await CreateAndLogDependencyExceptionAsync(failedSecureDataException);
            }
            catch (Exception exception)
            {
                var failedSecureDataServiceException = new FailedSecureDataServiceException(
                    message: "Failed secure data service error occurred, please contact support.",
                    innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSecureDataServiceException);
            }
        }

        private async ValueTask<SecureDataValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var secureDataValidationException = new SecureDataValidationException(
                message: "Secure data validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(secureDataValidationException);

            return secureDataValidationException;
        }

        private async ValueTask<SecureDataDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(
            Xeption exception)
        {
            var secureDataDependencyValidationException = new SecureDataDependencyValidationException(
                message: "Secure data dependency validation errors occurred, fix the errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(secureDataDependencyValidationException);

            return secureDataDependencyValidationException;
        }

        private async ValueTask<SecureDataDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var secureDataDependencyException = new SecureDataDependencyException(
                message: "Secure data dependency errors occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(secureDataDependencyException);

            return secureDataDependencyException;
        }

        private async ValueTask<SecureDataServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var secureDataServiceException = new SecureDataServiceException(
                message: "Secure data service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(secureDataServiceException);

            return secureDataServiceException;
        }
    }
}