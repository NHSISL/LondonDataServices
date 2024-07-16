// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.AddressCoordinations.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<ResolvedAddress> ReturningResolvedAddressFunction();
        private delegate ValueTask<List<Guid>> ReturningGuidListFunction();

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
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationDependencyException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentAddressCoordinationException invalidArgumentAddressCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationDependencyException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service aggregate error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedDecryptServiceException);
            }
        }

        private async ValueTask<ResolvedAddress> TryCatch(
            ReturningResolvedAddressFunction returningResolvedAddressFunction)
        {
            try
            {
                return await returningResolvedAddressFunction();
            }
            catch (InvalidArgumentAddressCoordinationException invalidArgumentAddressCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationDependencyException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service aggregate error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressCoordinationServiceException);
            }
        }

        private async ValueTask<List<Guid>> TryCatch(ReturningGuidListFunction returningGuidListFunction)
        {
            try
            {
                return await returningGuidListFunction();
            }
            catch (InvalidArgumentAddressCoordinationException invalidArgumentAddressCoordinationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressOrchestrationDependencyException);
            }
            catch (Exception exception)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressCoordinationServiceException);
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
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressCoordinationDependencyValidationException);

            return addressCoordinationDependencyValidationException;
        }

        private AddressCoordinationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var addressCoordinationDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressCoordinationDependencyException);

            return addressCoordinationDependencyException;
        }

        private AddressCoordinationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressCoordinationServiceException);

            return addressCoordinationServiceException;
        }
    }
}