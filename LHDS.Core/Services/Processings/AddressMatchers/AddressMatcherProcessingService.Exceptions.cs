// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.AddressMatcher.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private delegate string ReturningExtractPostcodeFunction();

        private string TryCatch(ReturningExtractPostcodeFunction returningExtractPostcodeFunction)
        {
            try
            {
                return returningExtractPostcodeFunction();
            }
            catch (InvalidArgumentAddressMatcherProcessingException invalidArgumentAddressMatcherProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressMatcherProcessingException);
            }
            catch (MultiplePostCodesAddressMatcherProcessingServiceException multiplePostCodesAddressMatcherProcessingServiceException)
            {
                throw CreateAndLogValidationException(multiplePostCodesAddressMatcherProcessingServiceException);
            }
            catch (PostCodeNotFoundAddressMatcherProcessingServiceException postCodeNotFoundAddressMatcherProcessingServiceException)
            {
                throw CreateAndLogValidationException(postCodeNotFoundAddressMatcherProcessingServiceException);
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