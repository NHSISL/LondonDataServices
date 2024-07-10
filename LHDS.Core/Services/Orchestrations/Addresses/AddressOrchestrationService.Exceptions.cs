// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using LHDS.Core.Models.Orchestrations.Addresses.Exceptions;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationService : IAddressOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentAddressOrchestrationException
                invalidArgumentAddressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressOrchestrationException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressDependencyValidationException);
            }
            catch (AssignValidationException assignValidationException)
            {
                throw CreateAndLogDependencyValidationException(assignValidationException);
            }
            catch (AssignDependencyValidationException assignDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(assignDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw CreateAndLogDependencyValidationException(csvHelperClientValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw CreateAndLogDependencyException(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw CreateAndLogDependencyException(addressServiceException);
            }
            catch (AssignDependencyException assignDependencyException)
            {
                throw CreateAndLogDependencyException(assignDependencyException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw CreateAndLogDependencyException(assignServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw CreateAndLogDependencyException(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw CreateAndLogDependencyException(csvHelperClientServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressOrchestrationServiceException =
                    new FailedAddressOrchestrationServiceException(
                        message: "Failed address aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressOrchestrationServiceException =
                    new FailedAddressOrchestrationServiceException(
                        message: "Failed address orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressOrchestrationServiceException);
            }
        }

        private AddressValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var addressValidationOrchestrationException =
                new AddressValidationOrchestrationException(
                    message: "Address orchestration validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressValidationOrchestrationException);

            return addressValidationOrchestrationException;
        }

        private AddressOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressOrchestrationDependencyValidationException =
                new AddressOrchestrationDependencyValidationException(
                    message: "Address orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressOrchestrationDependencyValidationException);

            return addressOrchestrationDependencyValidationException;
        }

        private AddressOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressOrchestrationDependencyException =
                new AddressOrchestrationDependencyException(
                    message: "Address orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressOrchestrationDependencyException);

            return addressOrchestrationDependencyException;
        }

        private AddressOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressOrchestrationServiceException);

            return addressOrchestrationServiceException;
        }
    }
}
