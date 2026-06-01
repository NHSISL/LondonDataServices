// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressToUprns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressToUprns
{
    public partial class AddressToUprnOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentAddressToUprnOrchestrationException invalidArgumentAddressToUprnOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentAddressToUprnOrchestrationException);
            }
            catch (AssignValidationException assignValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignValidationException);
            }
            catch (AssignDependencyValidationException assignDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(assignDependencyValidationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DocumentDependencyValidationException documentDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentDependencyValidationException);
            }
            catch (AddressToUprnFileLogValidationException addressToUprnFileLogValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(addressToUprnFileLogValidationException);
            }
            catch (AddressToUprnFileLogDependencyValidationException addressToUprnFileLogDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    addressToUprnFileLogDependencyValidationException);
            }
            catch (AssignDependencyException assignDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignDependencyException);
            }
            catch (AssignServiceException assignServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(assignServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (AddressToUprnFileLogDependencyException addressToUprnFileLogDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressToUprnFileLogDependencyException);
            }
            catch (AddressToUprnFileLogServiceException addressToUprnFileLogServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(addressToUprnFileLogServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressToUprnOrchestrationServiceException =
                    new FailedAddressToUprnOrchestrationServiceException(
                        message: "Failed address to UPRN orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressToUprnOrchestrationServiceException);
            }
        }

        private async ValueTask<AddressToUprnOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var addressToUprnOrchestrationValidationException =
                new AddressToUprnOrchestrationValidationException(
                    message: "Address to UPRN orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnOrchestrationValidationException);

            return addressToUprnOrchestrationValidationException;
        }

        private async ValueTask<AddressToUprnOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressToUprnOrchestrationDependencyValidationException =
                new AddressToUprnOrchestrationDependencyValidationException(
                    message: "Address to UPRN orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressToUprnOrchestrationDependencyValidationException);

            return addressToUprnOrchestrationDependencyValidationException;
        }

        private async ValueTask<AddressToUprnOrchestrationDependencyException>
            CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var addressToUprnOrchestrationDependencyException =
                new AddressToUprnOrchestrationDependencyException(
                    message: "Address to UPRN orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressToUprnOrchestrationDependencyException);

            return addressToUprnOrchestrationDependencyException;
        }

        private async ValueTask<AddressToUprnOrchestrationServiceException>
            CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var addressToUprnOrchestrationServiceException =
                new AddressToUprnOrchestrationServiceException(
                    message: "Address to UPRN orchestration service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnOrchestrationServiceException);

            return addressToUprnOrchestrationServiceException;
        }
    }
}
