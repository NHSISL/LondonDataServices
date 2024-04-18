// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    internal partial class AddressPersistanceOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressPersistanceOrchestrationException
                invalidArgumentAddressPersistanceOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressPersistanceOrchestrationException);
            }
            catch (AddressProcessingValidationException addressProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingValidationException);
            }
            catch (AddressProcessingDependencyValidationException addressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingDependencyValidationException);
            }
            catch (AddressProcessingDependencyException addressProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(addressProcessingDependencyException);
            }
            catch (AddressProcessingServiceException addressProcessingServiceException)
            {
                throw CreateAndLogDependencyException(addressProcessingServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressPersistanceOrchestrationServiceException =
                    new FailedAddressPersistanceOrchestrationServiceException(
                        message: "Failed address persistance orchestration service error occurred, " +
                        "please contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressPersistanceOrchestrationServiceException);
            }
        }
        private AddressPersistanceOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationValidationException =
                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation error occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationValidationException);

            return addressPersistanceOrchestrationValidationException;
        }

        private AddressPersistanceOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationDependencyValidationException =
                new AddressPersistanceOrchestrationDependencyValidationException(
                    message: "Address persistance orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressPersistanceOrchestrationDependencyValidationException);

            return addressPersistanceOrchestrationDependencyValidationException;
        }

        private AddressPersistanceOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressPersistanceOrchestrationDependencyException =
                new AddressPersistanceOrchestrationDependencyException(
                    message: "Address persistance orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressPersistanceOrchestrationDependencyException);

            throw addressPersistanceOrchestrationDependencyException;
        }

        private AddressPersistanceOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressPersistanceOrchestrationServiceException =
                new AddressPersistanceOrchestrationServiceException(
                    message: "Address persistance orchestration service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationServiceException);

            return addressPersistanceOrchestrationServiceException;
        }
    }
}
