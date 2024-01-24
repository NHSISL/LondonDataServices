// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Processings.AddressMatcher.Exceptions;
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
    }
}