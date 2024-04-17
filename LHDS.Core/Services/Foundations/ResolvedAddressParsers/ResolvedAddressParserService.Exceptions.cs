// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserService
    {
        private delegate ValueTask<List<ResolvedAddress>> ReturningResolvedAddressListFunction();

        private async ValueTask<List<ResolvedAddress>> TryCatch(
            ReturningResolvedAddressListFunction returningResolvedAddressListFunction)
        {
            try
            {
                return await returningResolvedAddressListFunction();
            }
            catch (InvalidArgumentResolvedAddressParserException invalidArgumentResolvedAddressParserException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressParserException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressParserServiceException =
                    new FailedResolvedAddressParserServiceException(
                        message: "Failed address parser service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressParserServiceException);
            }
        }

        private ResolvedAddressParserValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressParserValidationException =
                new ResolvedAddressParserValidationException(
                    message: "ResolvedAddress parser validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressParserValidationException);

            return addressParserValidationException;
        }

        private ResolvedAddressParserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressParserServiceException = new ResolvedAddressParserServiceException(
                message: "ResolvedAddress parser service error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(addressParserServiceException);

            return addressParserServiceException;
        }
    }
}