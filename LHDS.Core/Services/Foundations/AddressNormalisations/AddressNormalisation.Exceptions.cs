// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationService
    {
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<AddressNormalisation> TryCatch(
            ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidAddressNormalisationArgumentException invalidAddressNormalisationArgumentException)
            {
                throw CreateAndLogValidationException(invalidAddressNormalisationArgumentException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationServiceException =
                    new FailedAddressNormalisationServiceException(
                        message: "Failed address normalisation service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressNormalisationServiceException);
            }
        }

        private async ValueTask<string> TryCatch(
            ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidAddressNormalisationArgumentException invalidAddressNormalisationArgumentException)
            {
                throw CreateAndLogValidationException(invalidAddressNormalisationArgumentException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationServiceException =
                    new FailedAddressNormalisationServiceException(
                        message: "Failed address normalisation service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressNormalisationServiceException);
            }
        }

        private AddressNormalisationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
                new AddressNormalisationValidationException(
                    message: "Address normalisation validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }

        private AddressNormalisationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var addressNormalisationServiceException =
                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationServiceException);

            return addressNormalisationServiceException;
        }
    }
}