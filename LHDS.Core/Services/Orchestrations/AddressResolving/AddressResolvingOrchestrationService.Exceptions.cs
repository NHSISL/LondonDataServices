// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressResolvings
{
    internal partial class AddressResolvingOrchestrationService
    {
        private delegate ValueTask<AddressNormalisation> ReturningNormalisedAddressFunction();

        private async ValueTask<AddressNormalisation> TryCatch(ReturningNormalisedAddressFunction returningNormalisedAddressFunction)
        {
            try
            {
                return await returningNormalisedAddressFunction();
            }
            catch (InvalidArgumentAddressResolvingOrchestrationException invalidArgumentAddressResolvingOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressResolvingOrchestrationException);
            }
            catch (AddressProcessingValidationException addressProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingValidationException);
            }
            catch (AddressProcessingDependencyValidationException addressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressProcessingDependencyValidationException);
            }
            catch (AddressMatcherProcessingValidationException addressMatcherProcessingValidationException)
            {
                throw CreateAndLogDependencyValidationException(addressMatcherProcessingValidationException);
            }
            catch (ResolvedAddressValidationException resolvedAddressValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressValidationException);
            }
            catch (ResolvedAddressProcessingDependencyValidationException resolvedAddressProcessingDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(resolvedAddressProcessingDependencyValidationException);
            }
            catch (AddressProcessingDependencyException addressProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(addressProcessingDependencyException);
            }
            catch (AddressProcessingServiceException addressProcessingServiceException)
            {
                throw CreateAndLogDependencyException(addressProcessingServiceException);
            }
            catch (AddressMatcherProcessingServiceException addressMatcherProcessingServiceException)
            {
                throw CreateAndLogDependencyException(addressMatcherProcessingServiceException);
            }
            catch (ResolvedAddressProcessingDependencyException resolvedAddressProcessingDependencyException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingDependencyException);
            }
            catch (ResolvedAddressProcessingServiceException resolvedAddressProcessingServiceException)
            {
                throw CreateAndLogDependencyException(resolvedAddressProcessingServiceException);
            }
        }
        private AddressResolvingOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressResolvingOrchestrationValidationException =
                new AddressResolvingOrchestrationValidationException(
                    message: "Normalised address resolving orchestration validation error occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(addressResolvingOrchestrationValidationException);

            return addressResolvingOrchestrationValidationException;
        }

        private AddressResolvingOrchestrationDependencyValidationException
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressResolvingOrchestrationDependencyValidationException =
                new AddressResolvingOrchestrationDependencyValidationException(
                    message: "Normalised address resolving orchestration dependency validation error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressResolvingOrchestrationDependencyValidationException);

            return addressResolvingOrchestrationDependencyValidationException;
        }

        private AddressResolvingOrchestrationDependencyException
           CreateAndLogDependencyException(Xeption exception)
        {
            var addressResolvingOrchestrationDependencyException =
                new AddressResolvingOrchestrationDependencyException(
                    message: "Address resolving orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            this.loggingBroker.LogError(addressResolvingOrchestrationDependencyException);

            throw addressResolvingOrchestrationDependencyException;
        }
    }
}
