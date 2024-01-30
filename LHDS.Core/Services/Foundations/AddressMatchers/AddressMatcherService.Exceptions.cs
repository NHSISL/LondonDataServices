// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}
