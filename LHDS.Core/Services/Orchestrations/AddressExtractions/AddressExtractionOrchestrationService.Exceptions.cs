// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

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
        }

        private AddressExtractionOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressExtractionOrchestrationValidationException =
                new AddressExtractionOrchestrationValidationException(
                    message: "Address extraction orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionOrchestrationValidationException);

            return addressExtractionOrchestrationValidationException;
        }
    }
}