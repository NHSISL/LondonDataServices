// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationService
    {
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidArgumentAddressPersistanceOrchestrationException
                invalidArgumentAddressPersistanceOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAddressPersistanceOrchestrationException);
            }
        }
        private AddressPersistanceOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressPersistanceOrchestrationValidationException =
                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation errors occured, please try again",
                    innerException: exception);

            this.loggingBroker.LogError(addressPersistanceOrchestrationValidationException);

            return addressPersistanceOrchestrationValidationException;
        }
    }
}
