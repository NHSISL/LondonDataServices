// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingService : IAddressMatcherProcessingService
    {
        private delegate string ReturningStringFunction();
        private delegate ValueTask<HashSet<AddressMatch>> ReturningAddressMatchHashSetFunction();
        private delegate ValueTask<AddressMatch> ReturningAddressMatchFunction();

        private string TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return returningStringFunction();
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

        private async ValueTask<HashSet<AddressMatch>> TryCatch(
            ReturningAddressMatchHashSetFunction returningAddressMatchHashSetFunction)
        {
            try
            {
                return await returningAddressMatchHashSetFunction();
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

        private async ValueTask<AddressMatch> TryCatch(
            ReturningAddressMatchFunction returningAddressMatchFunction)
        {
            try
            {
                return await returningAddressMatchFunction();
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
