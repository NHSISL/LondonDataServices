// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions;
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
    }
}
