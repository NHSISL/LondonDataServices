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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationDependencyException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationDependencyException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service aggregate error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationDependencyException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service aggregate error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedAddressCoordinationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressCoordinationServiceException);
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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressCoordinationException);
            }
            catch (AddressValidationOrchestrationException addressValidationOrchestrationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationOrchestrationException);
            }
            catch (AddressOrchestrationDependencyValidationException
                addressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    addressOrchestrationDependencyValidationException);
            }
            catch (ResolvedAddressOrchestrationValidationException
                resolvedAddressOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(resolvedAddressOrchestrationValidationException);
            }
            catch (ResolvedAddressOrchestrationDependencyValidationException
                resolvedAddressOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    resolvedAddressOrchestrationDependencyValidationException);
            }
            catch (AddressOrchestrationServiceException addressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationServiceException);
            }
            catch (AddressOrchestrationDependencyException
                addressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressOrchestrationDependencyException);
            }
            catch (ResolvedAddressOrchestrationServiceException resolvedAddressOrchestrationServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationServiceException);
            }
            catch (ResolvedAddressOrchestrationDependencyException
                resolvedAddressOrchestrationDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(resolvedAddressOrchestrationDependencyException);
            }
            catch (Exception exception)
            {
                var failedAddressCoordinationServiceException =
                    new FailedAddressCoordinationServiceException(
                        message: "Failed address coordination service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressCoordinationServiceException);
            }
        }

        private async ValueTask<AddressCoordinationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var addressCoordinationValidationException =
                new AddressCoordinationValidationException(
                    message: "Address coordination validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressCoordinationValidationException);

            return addressCoordinationValidationException;
        }

        private async ValueTask<AddressCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressCoordinationDependencyValidationException =
                new AddressCoordinationDependencyValidationException(
                    message: "Address coordination dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressCoordinationDependencyValidationException);

            return addressCoordinationDependencyValidationException;
        }

        private async ValueTask<AddressCoordinationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var addressCoordinationDependencyException =
                new AddressCoordinationDependencyException(
                    message: "Address coordination dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressCoordinationDependencyException);

            return addressCoordinationDependencyException;
        }

        private async ValueTask<AddressCoordinationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var addressCoordinationServiceException =
                new AddressCoordinationServiceException(
                    message: "Address coordination service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressCoordinationServiceException);

            return addressCoordinationServiceException;
        }
    }
}