// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Numerics;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Clients.LibPostalClient.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisation.Exceptions;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Foundations.DataSets.Exceptions;
using NEL.LibPostalClient.Models.Clients.LibPostal.Exceptions;
using Xeptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public partial class AddressNormalisationService
    {
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<AddressNormalisation> TryCatch(
            ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidAddressNormalisationArgumentException invalidAddressNormalisationArgumentException)
            {
                throw CreateAndLogValidationException(invalidAddressNormalisationArgumentException);
            }
            catch (LibPostalClientValidationException libPostalClientValidationException)
            {
                var invalidLibPostalValidationException =
                    new InvalidLibPostalValidationException(
                        message: "Invalid lib poastal validation error occurred.",
                        innerException: libPostalClientValidationException,
                        data: libPostalClientValidationException.Data);

                throw CreateAndLogDependencyValidationException(libPostalClientValidationException);
            }
            catch (LibPostalClientDependencyException libPostalClientDependencyException)
            {
                var invalidLibPostalDependencyException =
                    new InvalidLibPostalDependencyException(
                        message: "Invalid lib poastal dependency error occurred.",
                        innerException: libPostalClientDependencyException);

                throw CreateAndLogDependencyException(libPostalClientDependencyException);
            }
            catch (LibPostalClientServiceException libPostalClientServiceException)
            {
                var invalidLibPostalDependencyException =
                    new InvalidLibPostalDependencyException(
                        message: "Invalid lib poastal dependency error occurred.",
                        innerException: libPostalClientServiceException);

                throw CreateAndLogDependencyException(libPostalClientServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationServiceException =
                    new FailedAddressNormalisationServiceException(
                        message: "Failed address normalisation service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressNormalisationServiceException);
            }
        }

        private async ValueTask<string> TryCatch(
            ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (InvalidAddressNormalisationArgumentException invalidAddressNormalisationArgumentException)
            {
                throw CreateAndLogValidationException(invalidAddressNormalisationArgumentException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationServiceException =
                    new FailedAddressNormalisationServiceException(
                        message: "Failed address normalisation service occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressNormalisationServiceException);
            }
        }

        private AddressNormalisationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
            new AddressNormalisationValidationException(
                    message: "Address normalisation validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }

        private AddressNormalisationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressNormalisationDependencyValidationException =
                new AddressNormalisationDependencyValidationException(
                    message: "Address normalisation dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationDependencyValidationException);

            return addressNormalisationDependencyValidationException;
        }

        private AddressNormalisationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var addressNormalisationDependencyException =
                new AddressNormalisationDependencyException(
                    message: "Address normalisation dependency error occurred, fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationDependencyException);

            return addressNormalisationDependencyException;
        }

        private AddressNormalisationServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var addressNormalisationServiceException =
                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationServiceException);

            return addressNormalisationServiceException;
        }
    }
}