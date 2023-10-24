// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService
    {
        private delegate Task<List<Address>> ReturningAddressListFunction();

        private async Task<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (NullAddressParserException nullAddressParserException)
            {
                throw CreateAndLogValidationException(nullAddressParserException);
            }
        }

        private AddressParserValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressParserValidationException =
                new AddressParserValidationException(
                    message: "Address parser validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressParserValidationException);

            return addressParserValidationException;
        }
    }
}