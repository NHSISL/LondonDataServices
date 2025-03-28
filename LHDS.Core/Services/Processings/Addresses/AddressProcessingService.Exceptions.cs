// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        private delegate ValueTask<T?> ReturningFunction<T>();

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullAddressProcessingException nullAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAddressException);
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressProcessingServiceException);
            }
        }

        private async ValueTask<T?> TryCatch<T>(
            ReturningFunction<T> returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (NullAddressProcessingException nullAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAddressException);
            }
            catch (InvalidArgumentAddressProcessingException invalidArgumentAddressProcessingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressProcessingException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressDependencyValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressProcessingServiceException =
                    new FailedAddressProcessingServiceException(
                        message: "Failed Address processing service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressProcessingServiceException);
            }
        }

        private async ValueTask<AddressProcessingValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var addressProcessingValidationExceptionn =
                new AddressProcessingValidationException(
                    message: "Address processing validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressProcessingValidationExceptionn);

            return addressProcessingValidationExceptionn;
        }

        private async ValueTask<AddressProcessingDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(
            Xeption exception)
        {
            var addressProcessingDependencyValidationException =
                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressProcessingDependencyValidationException);

            return addressProcessingDependencyValidationException;
        }

        private async ValueTask<AddressProcessingDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var addressProcessingDependencyException =
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException: exception?.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressProcessingDependencyException);

            throw addressProcessingDependencyException;
        }

        private async ValueTask<AddressProcessingServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var addressProcessingServiceException = new
                AddressProcessingServiceException(
                    message: "Address processing service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressProcessingServiceException);

            return addressProcessingServiceException;
        }
    }
}
