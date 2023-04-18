// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private delegate ValueTask ReturningRetrieveOptOutStatusFunction();

        private async ValueTask TryCatch(ReturningRetrieveOptOutStatusFunction returningRetrieveOptOutFunction)
        {
            try
            {
                await returningRetrieveOptOutFunction();
            }
            catch (InvalidArgumentOptOutOrchestrationException invalidArgumentRetieveOptOutStatusOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentRetieveOptOutStatusOrchestrationException);
            }
        }

        private OptOutOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            string validationSummary = GetValidationSummary(exception.Data);

            var decryptionOrchestrationValidationException =
                new OptOutOrchestrationValidationException(exception, validationSummary);

            this.loggingBroker.LogError(decryptionOrchestrationValidationException);

            return decryptionOrchestrationValidationException;
        }

        private OptOutOrchestrationDependencyValidationException
           CreateAndLogDependencyValidationException(Xeption exception)
        {
            var retrieveOptOutStatusOrchestrationDependencyValidationException =
                new OptOutOrchestrationDependencyValidationException(exception.InnerException as Xeption);
            this.loggingBroker.LogError(retrieveOptOutStatusOrchestrationDependencyValidationException);

            return retrieveOptOutStatusOrchestrationDependencyValidationException;
        }
    }
}
