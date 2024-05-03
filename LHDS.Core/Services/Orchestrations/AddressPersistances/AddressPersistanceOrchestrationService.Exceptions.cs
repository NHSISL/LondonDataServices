// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    internal partial class AddressPersistanceOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();
        private delegate ValueTask<Address> ReturningAddressFunction();
        private delegate ValueTask<ResolvedAddress> ReturningResolvedFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressPersistenceOrchestrationException invalidArgumentAddressPersistanceOrchestrationException)
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
            catch (AggregateException aggregateException)
            {
                var failedAddressPersistanceOrchestrationServiceException =
                    new FailedAddressPersistenceOrchestrationServiceException(
                        message: "Failed address persistence aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressPersistanceOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressPersistanceOrchestrationServiceException =
                    new FailedAddressPersistenceOrchestrationServiceException(
                        message: "Failed address persistence orchestration service error occurred, contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressPersistanceOrchestrationServiceException);
            }
        }

        private async ValueTask<Address> TryCatch(ReturningAddressFunction returningAddressFunction)
        {
            try
            {
                return await returningAddressFunction();
            }
            catch (InvalidArgumentAddressPersistenceOrchestrationException invalidArgumentAddressPersistanceOrchestrationException)
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
            catch (AggregateException aggregateException)
            {
                var failedAddressPersistanceOrchestrationServiceException =
                    new FailedAddressPersistenceOrchestrationServiceException(
                        message: "Failed address persistence aggregate processing service error occurred, " +
                        "contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressPersistanceOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressPersistanceOrchestrationServiceException =
                    new FailedAddressPersistenceOrchestrationServiceException(
                        message: "Failed address persistence orchestration service error occurred, " +
                        "contact support",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressPersistanceOrchestrationServiceException);
            }
        }

        private async ValueTask<ResolvedAddress> TryCatch(ReturningResolvedFunction returningResolvedFunction)
        {
            try
            {
                return await returningResolvedFunction();
            }
            catch (InvalidArgumentAddressPersistenceOrchestrationException invalidArgumentAddressPersistanceOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressPersistanceOrchestrationException);
            }
        }
        private AddressPersistenceOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationValidationException =
                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistence orchestration validation error occurred, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationValidationException);

            return addressPersistanceOrchestrationValidationException;
        }

        private AddressPersistenceOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationDependencyValidationException =
                new AddressPersistenceOrchestrationDependencyValidationException(
                    message: "Address persistence orchestration dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressPersistanceOrchestrationDependencyValidationException);

            return addressPersistanceOrchestrationDependencyValidationException;
        }

        private AddressPersistenceOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressPersistanceOrchestrationDependencyException =
                new AddressPersistenceOrchestrationDependencyException(
                    message: "Address persistence orchestration dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressPersistanceOrchestrationDependencyException);

            throw addressPersistanceOrchestrationDependencyException;
        }

        private AddressPersistenceOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressPersistanceOrchestrationServiceException =
                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistence orchestration service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationServiceException);

            return addressPersistanceOrchestrationServiceException;
        }
    }
}
