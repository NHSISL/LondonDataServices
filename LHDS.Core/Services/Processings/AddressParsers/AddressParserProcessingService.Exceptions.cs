// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressParserProcessingException invalidArgumentAddressParserProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressParserProcessingException);
            }
        }

        private AddressParserProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressParserProcessingProcessingValidationException =
                new AddressParserProcessingValidationException(
                    message: "Address parser processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressParserProcessingProcessingValidationException);

            return addressParserProcessingProcessingValidationException;
        }
    }
}