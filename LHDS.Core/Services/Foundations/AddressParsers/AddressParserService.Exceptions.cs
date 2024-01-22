// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressParserException invalidArgumentAddressParserException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressParserException);
            }
            catch (Exception exception)
            {
                var failedAddressParserServiceException =
                    new FailedAddressParserServiceException(
                        message: "Failed address parser service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressParserServiceException);
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

        private AddressParserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressParserServiceException = new AddressParserServiceException(
                message: "Address parser service error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(addressParserServiceException);

            return addressParserServiceException;
        }
    }
}