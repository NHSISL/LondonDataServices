// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Processings.Addresses.Exceptions;
using System;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private delegate string ReturningCleanedAddressFunction();

        private string TryCatch(ReturningCleanedAddressFunction returningCleanedAddressFunction)
        {
            try
            {
                return returningCleanedAddressFunction();
            }
            catch (InvalidArgumentAddressMatcherProcessingException invalidArgumentAddressMatcherProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressMatcherProcessingException);
            }
            catch (Exception exception)
            {
                var failedAddressMatcherProcessingServiceException =
                    new FailedAddressMatcherProcessingServiceException(
                        message: "Failed address matcher processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressMatcherProcessingServiceException);
            }
        }

        private AddressMatcherProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressMatcherProcessingValidationException =
                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressMatcherProcessingValidationException);

            return addressMatcherProcessingValidationException;
        }

        private AddressMatcherProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressMatcherProcessingServiceException = new
                AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressMatcherProcessingServiceException);

            return addressMatcherProcessingServiceException;
        }
    }
}
