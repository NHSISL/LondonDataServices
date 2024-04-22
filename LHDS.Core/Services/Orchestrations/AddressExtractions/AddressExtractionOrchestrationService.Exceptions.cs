// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();
        private delegate ValueTask<Address> ReturningAddressFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressExtractionOrchestrationException
                invalidArgumentAddressExtractionOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressExtractionOrchestrationException);
            }
            catch (AddressParserValidationException addressParserValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressParserValidationException);
            }
            catch (AddressParserDependencyValidationException addressParserDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressParserDependencyValidationException);
            }
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressParserDependencyException addressParserDependencyException)
            {
                throw CreateAndLogDependencyException(addressParserDependencyException);
            }
            catch (AddressParserServiceException addressParserServiceException)
            {
                throw CreateAndLogDependencyException(addressParserServiceException);
            }
            catch (AddressNormalisationDependencyException addressNormalisationDependencyException)
            {
                throw CreateAndLogDependencyException(addressNormalisationDependencyException);
            }
            catch (AddressNormalisationServiceException addressNormalisationServiceException)
            {
                throw CreateAndLogDependencyException(addressNormalisationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressExtractionOrchestrationServiceException =
                    new FailedAddressExtractionOrchestrationServiceException(
                        message: "Failed address extraction aggregate orchestration service occurred, " +
                            "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressExtractionOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressExtractionOrchestrationServiceException =
                    new FailedAddressExtractionOrchestrationServiceException(
                        message: "Failed address extraction orchestration service error occurred, " +
                            "please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressExtractionOrchestrationServiceException);
            }
        }

        private async ValueTask<Address> TryCatch(ReturningAddressFunction returningAddressFunction)
        {
            try
            {
                return await returningAddressFunction();
            }
            catch (InvalidArgumentAddressExtractionOrchestrationException
                invalidArgumentAddressExtractionOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressExtractionOrchestrationException);
            }
            catch (AddressParserValidationException addressParserValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressParserValidationException);
            }
            catch (AddressParserDependencyValidationException addressParserDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressParserDependencyValidationException);
            }
            catch (AddressNormalisationValidationException addressNormalisationValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationValidationException);
            }
            catch (AddressNormalisationDependencyValidationException addressNormalisationDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressNormalisationDependencyValidationException);
            }
            catch (AddressParserDependencyException addressParserDependencyException)
            {
                throw CreateAndLogDependencyException(addressParserDependencyException);
            }
            catch (AddressParserServiceException addressParserServiceException)
            {
                throw CreateAndLogDependencyException(addressParserServiceException);
            }
            catch (AddressNormalisationDependencyException addressNormalisationDependencyException)
            {
                throw CreateAndLogDependencyException(addressNormalisationDependencyException);
            }
            catch (AddressNormalisationServiceException addressNormalisationServiceException)
            {
                throw CreateAndLogDependencyException(addressNormalisationServiceException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressExtractionOrchestrationServiceException =
                    new FailedAddressExtractionOrchestrationServiceException(
                        message: "Failed address extraction aggregate orchestration service occurred, " +
                        "please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressExtractionOrchestrationServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressExtractionOrchestrationServiceException =
                    new FailedAddressExtractionOrchestrationServiceException(
                        message: "Failed address extraction orchestration service error occurred, " +
                        "please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressExtractionOrchestrationServiceException);
            }
        }

        private AddressExtractionValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var addressExtractionValidationOrchestrationException =
                new AddressExtractionValidationOrchestrationException(
                    message: "Address extraction orchestration validation error occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionValidationOrchestrationException);

            return addressExtractionValidationOrchestrationException;
        }

        private AddressExtractionOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressExtractionOrchestrationDependencyValidationException =
                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressExtractionOrchestrationDependencyValidationException);

            return addressExtractionOrchestrationDependencyValidationException;
        }

        private AddressExtractionOrchestrationDependencyException
            CreateAndLogDependencyException(Xeption exception)
        {
            var addressExtractionOrchestrationDependencyException =
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressExtractionOrchestrationDependencyException);

            throw addressExtractionOrchestrationDependencyException;
        }

        private AddressExtractionOrchestrationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var addressExtractionOrchestrationServiceException =
                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionOrchestrationServiceException);

            return addressExtractionOrchestrationServiceException;
        }
    }
}