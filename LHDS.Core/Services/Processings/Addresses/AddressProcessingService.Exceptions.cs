// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Addresses
{
    public partial class AddressProcessingService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<Address> ReturningAddressProcessingFunction();
        private delegate ValueTask<bool> ReturningBooleanProcessingFunction();
        private delegate IQueryable<Address> ReturningAddressesFunction();
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullAddressProcessingException nullAddressException)
            {
                throw CreateAndLogValidationException(nullAddressException);
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressProcessingServiceException);
            }
        }

        private async ValueTask<Address> TryCatch(
            ReturningAddressProcessingFunction returningAddressProcessingFunction)
        {
            try
            {
                return await returningAddressProcessingFunction();
            }
            catch (NullAddressProcessingException nullAddressException)
            {
                throw CreateAndLogValidationException(nullAddressException);
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressProcessingServiceException);
            }
        }

        private async ValueTask<bool> TryCatch(
            ReturningBooleanProcessingFunction returningBooleanProcessingFunction)
        {
            try
            {
                return await returningBooleanProcessingFunction();
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressProcessingServiceException);
            }
        }

        private IQueryable<Address> TryCatch(ReturningAddressesFunction returningAddressesFunction)
        {
            try
            {
                return returningAddressesFunction();
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressProcessingServiceException);
            }
        }

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressProcessingServiceException);
            }
        }

        private AddressProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressProcessingValidationExceptionn =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressProcessingValidationExceptionn);

            return addressProcessingValidationExceptionn;
        }

        private AddressProcessingDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var addressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressProcessingDependencyValidationException);

            return addressProcessingDependencyValidationException;
        }

        private AddressProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var addressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            this.loggingBroker.LogError(addressProcessingDependencyException);

            throw addressProcessingDependencyException;
        }

        private AddressProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressProcessingServiceException = new
                AddressProcessingServiceException(
                    message: "Address processing service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressProcessingServiceException);

            return addressProcessingServiceException;
        }
    }
}
