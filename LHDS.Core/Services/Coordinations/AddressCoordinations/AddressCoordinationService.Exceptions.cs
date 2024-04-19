// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressCoordinationException invalidArgumentAddressCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressCoordinationException);
            }
            catch (AddressExtractionValidationOrchestrationException addressExtractionValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(addressExtractionValidationOrchestrationException);
            }
            catch (AddressExtractionOrchestrationDependencyValidationException
                addressExtractionOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressExtractionOrchestrationDependencyValidationException);
            }
            catch (AddressPersistenceOrchestrationValidationException
                addressPersistanceOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressPersistanceOrchestrationValidationException);
            }
            catch (AddressPersistenceOrchestrationDependencyValidationException
                addressPersistanceOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressPersistanceOrchestrationDependencyValidationException);
            }
            catch (AddressExtractionOrchestrationServiceException addressExtractionOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressExtractionOrchestrationServiceException);
            }
            catch (AddressExtractionOrchestrationDependencyException
                addressExtractionOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressExtractionOrchestrationDependencyException);
            }
            catch (AddressPersistenceOrchestrationServiceException addressPersistanceOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressPersistanceOrchestrationServiceException);
            }
            catch (AddressPersistenceOrchestrationDependencyException
                addressPersistanceOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressPersistanceOrchestrationDependencyException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private AddressCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationValidationException);

            return addressCoordinationValidationException;
        }

        private AddressCoordinationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressCoordinationDependencyValidationException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationDependencyValidationException);

            return addressCoordinationDependencyValidationException;
        }

        private AddressCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var addressCoordinationDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationDependencyException);

            return addressCoordinationDependencyException;
        }

        private AddressCoordinationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationServiceException);

            return addressCoordinationServiceException;
        }
    }
}