// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Processings.AddressNormalisations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationService
    {
        private delegate ValueTask<List<AddressNormalisation>> ReturningAddressNormalisationListFunction();
        private delegate ValueTask<AddressNormalisation> ReturningAddressNormalisationFunction();

        private async ValueTask<List<AddressNormalisation>> TryCatch(
            ReturningAddressNormalisationListFunction returningAddressNormalisationListFunction)
        {
            try
            {
                return await returningAddressNormalisationListFunction();
            }
            catch (InvalidArgumentAddressNormalisationOrchestrationException invalidArgumentAddressNormalisationOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressNormalisationOrchestrationException);
            }
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
             catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressNormalisationDependencyException addressNormalisationDependencyException)
            {
                throw CreateAndLogDependencyException(addressNormalisationDependencyException);
            }
            catch (AddressNormalisationServiceException addressNormalisationServiceException)
            {
                throw CreateAndLogDependencyException(addressNormalisationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationOrchestrationServiceException =
                    new FailedAddressNormalisationOrchestrationServiceException(
                        message: "Failed address normalisation orchestration service error occurred, contact support.",
                        exception);

                throw CreateAndLogServiceException(failedAddressNormalisationOrchestrationServiceException);
            }
        }

        private async ValueTask<AddressNormalisation> TryCatch(
            ReturningAddressNormalisationFunction returningAddressNormalisationFunction)
        {
            try
            {
                return await returningAddressNormalisationFunction();
            }
            catch (InvalidArgumentAddressNormalisationOrchestrationException invalidArgumentAddressNormalisationOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressNormalisationOrchestrationException);
            }
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressNormalisationDependencyException addressNormalisationDependencyException)
            {
                throw CreateAndLogDependencyException(addressNormalisationDependencyException);
            }
            catch (AddressNormalisationServiceException addressNormalisationServiceException)
            {
                throw CreateAndLogDependencyException(addressNormalisationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressNormalisationOrchestrationServiceException =
                    new FailedAddressNormalisationOrchestrationServiceException(
                        message: "Failed address normalisation orchestration service error occurred, contact support.",
                        exception);

                throw CreateAndLogServiceException(failedAddressNormalisationOrchestrationServiceException);
            }
        }

        private AddressNormalisationOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressNormalisationOrchestrationValidationException =
                new AddressNormalisationOrchestrationValidationException(
                    message: "Address normalisation orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressNormalisationOrchestrationValidationException);

            return addressNormalisationOrchestrationValidationException;
        }

        private AddressNormalisationOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressNormalisationOrchestrationDependencyValidationException =
                new AddressNormalisationOrchestrationDependencyValidationException(
                    message: "Address normalisation orchestration dependency validation occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressNormalisationOrchestrationDependencyValidationException);

            return addressNormalisationOrchestrationDependencyValidationException;
        }

        private AddressNormalisationOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressNormalisationOrchestrationDependencyException =
                new AddressNormalisationOrchestrationDependencyException(
                    message: "Address normalisation orchestration dependency error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressNormalisationOrchestrationDependencyException);

            throw addressNormalisationOrchestrationDependencyException;
        }

        private AddressNormalisationOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressNormalisationOrchestrationServiceException = new
                AddressNormalisationOrchestrationServiceException(
                message: "Address normalisation orchestration service error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogError(addressNormalisationOrchestrationServiceException);

            return addressNormalisationOrchestrationServiceException;
        }
    }
}