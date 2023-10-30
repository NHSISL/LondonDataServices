// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            catch (AddressExtractionOrchestrationValidationException addressExtractionOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressExtractionOrchestrationValidationException);
            }
            catch (AddressExtractionOrchestrationDependencyValidationException
                addressExtractionOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressExtractionOrchestrationDependencyValidationException);
            }
            catch (AddressPersistanceOrchestrationValidationException 
                addressPersistanceOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressPersistanceOrchestrationValidationException);
            }
            catch (AddressPersistanceOrchestrationDependencyValidationException 
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
            catch (AddressPersistanceOrchestrationServiceException addressPersistanceOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressPersistanceOrchestrationServiceException);
            }
            catch (AddressPersistanceOrchestrationDependencyException
                addressPersistanceOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressPersistanceOrchestrationDependencyException);
            }
        }

        private AddressCoordinationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationValidationException);

            return addressCoordinationValidationException;
        }

        private AddressCoordinationDependencyValidationException 
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressCoordinationDependencyValidationException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationDependencyValidationException);

            return addressCoordinationDependencyValidationException;
        }

        private AddressCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var addressCoordinationDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationDependencyException);

            return addressCoordinationDependencyException;
        }
    }
}