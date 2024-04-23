// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.ResolvedAddresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentResolvedAddressOrchestrationException
                invalidArgumentResolvedAddressOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentResolvedAddressOrchestrationException);
            }
        }

        private ResolvedAddressOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var resolvedAddressOrchestrationValidationException =
                new ResolvedAddressOrchestrationValidationException(
                    message: "Resolved address validation errors occured, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressOrchestrationValidationException);

            return resolvedAddressOrchestrationValidationException;
        }
    }
}
