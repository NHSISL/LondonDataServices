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
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressOrchestrationException);
            }
            catch (AddressValidationException addressValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressValidationException);
            }
            catch (AddressDependencyValidationException addressDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressDependencyValidationException);
            }
            catch (AssignValidationException assignValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignValidationException);
            }
            catch (AssignDependencyValidationException assignDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignDependencyValidationException);
            }
            catch (CsvHelperClientValidationException csvHelperClientValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(csvHelperClientValidationException);
            }
            catch (AddressDependencyException addressDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressDependencyException);
            }
            catch (AddressServiceException addressServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressServiceException);
            }
            catch (AssignDependencyException assignDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignDependencyException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignServiceException);
            }
            catch (CsvHelperClientDependencyException csvHelperClientDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientDependencyException);
            }
            catch (CsvHelperClientServiceException csvHelperClientServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(csvHelperClientServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressOrchestrationServiceException =
                    new FailedAddressOrchestrationServiceException(
                        message: "Failed address aggregate orchestration service error occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedAddressOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressOrchestrationServiceException =
                    new FailedAddressOrchestrationServiceException(
                        message: "Failed address orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressOrchestrationServiceException);
            }
        }

        private async ValueTask<AddressValidationOrchestrationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var addressValidationOrchestrationException =
                new AddressValidationOrchestrationException(
                    message: "Address orchestration validation error occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressValidationOrchestrationException);

            return addressValidationOrchestrationException;
        }

        private async ValueTask<AddressOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressOrchestrationDependencyValidationException =
                new AddressOrchestrationDependencyValidationException(
                    message: "Address orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressOrchestrationDependencyValidationException);

            return addressOrchestrationDependencyValidationException;
        }

        private async ValueTask<AddressOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var addressOrchestrationDependencyException =
                new AddressOrchestrationDependencyException(
                    message: "Address orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressOrchestrationDependencyException);

            return addressOrchestrationDependencyException;
        }

        private async ValueTask<AddressOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var addressOrchestrationServiceException =
                new AddressOrchestrationServiceException(
                    message: "Address orchestration service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressOrchestrationServiceException);

            return addressOrchestrationServiceException;
        }
    }
}
