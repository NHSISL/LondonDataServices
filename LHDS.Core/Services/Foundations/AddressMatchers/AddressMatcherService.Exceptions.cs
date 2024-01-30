// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherService
    {
        private delegate BestMatchEnum ReturningBestMatchEnumFunction();

        private BestMatchEnum TryCatch(
            ReturningBestMatchEnumFunction returningBestMatchEnumFunction)
        {
            try
            {
                return returningBestMatchEnumFunction();
            }
            catch (InvalidArgumentAddressMatcherException invalidArgumentAddressMatcherException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressMatcherException);
            }
            catch (Exception exception)
            {
                var failedAddressMatcherServiceException =
                    new FailedAddressMatcherServiceException(
                        message: "Failed address matcher service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressMatcherServiceException);
            }
        }

        private AddressMatcherValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressMatcherValidationException =
                new AddressMatcherValidationException(
                    message: "Address matcher validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressMatcherValidationException);

            return addressMatcherValidationException;
        }

        private AddressMatcherServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressMatcherServiceException = new
                AddressMatcherServiceException(
                    message: "Address matcher service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressMatcherServiceException);

            return addressMatcherServiceException;
        }
    }
}
