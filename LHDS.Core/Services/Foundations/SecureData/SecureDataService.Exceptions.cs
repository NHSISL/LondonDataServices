// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SecureDatas
{
    public partial class SecureDataService
    {
        private delegate ValueTask<SecureData> ReturningSecureDataFunction();

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
            catch (ArgumentException argumentException)
            {
                throw CreateAndLogDependencyValidationException(argumentException);
            }
            catch (Exception exception)
            {
                var failedSecureDataServiceException = new FailedSecureDataServiceException(
                    message: "Failed secure data service occurred, please contact support",
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

        private SecureDataDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var secureDataDependencyValidationException = new SecureDataDependencyValidationException(
                message: "Secure data dependency validation errors occurred, fix the errors and try again.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataDependencyValidationException);

            return secureDataDependencyValidationException;
        }

        private SecureDataServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var secureDataServiceException = new SecureDataServiceException(
                message: "Secure data service error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(secureDataServiceException);

            return secureDataServiceException;
        }
    }
}