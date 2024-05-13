// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingService
    {
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();

        private async ValueTask<AddressNormalisation> TryCatch(ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidArgumentAddressNormalisationProcessingException
        invalidArgumentAddressNormalisationProcessingException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressNormalisationProcessingException);
            }
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressNormalisationDependencyException addressNormalisationDependencyException)
            {
                throw CreateAndLogDependencyException(addressNormalisationDependencyException);
            }
            catch (AddressNormalisationServiceException addressNormalisationServiceException)
            {
                throw CreateAndLogDependencyException(addressNormalisationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationProcessingServiceException =
                    new FailedAddressNormalisationProcessingServiceException(
                        message: "Failed address normalisation processing service error occurred, please contact support.",
                        exception);

                throw CreateAndLogServiceException(failedAddressNormalisationProcessingServiceException);
            }
        }

        private AddressNormalisationProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressNormalisationProcessingValidationException =
                new AddressNormalisationProcessingValidationException(
                    message: "Address normalisation processing validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationProcessingValidationException);

            return addressNormalisationProcessingValidationException;
        }

        private AddressNormalisationProcessingDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressNormalisationProcessingDependencyValidationException =
                new AddressNormalisationProcessingDependencyValidationException(
                    message: "Address normalisation processing dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressNormalisationProcessingDependencyValidationException);

            return addressNormalisationProcessingDependencyValidationException;
        }

        private AddressNormalisationProcessingDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressNormalisationProcessingDependencyException =
                new AddressNormalisationProcessingDependencyException(
                    message: "Address normalisation processing dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressNormalisationProcessingDependencyException);

            throw addressNormalisationProcessingDependencyException;
        }

        private AddressNormalisationProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressNormalisationProcessingServiceException = new
                AddressNormalisationProcessingServiceException(
                message: "Address normalisation processing service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(addressNormalisationProcessingServiceException);

            return addressNormalisationProcessingServiceException;
        }
    }
}