// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Orchestrations.TerminologyMedata.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentTerminologyMetaDataOrchestrationException
                invalidArgumentTerminologyMetaDataOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentTerminologyMetaDataOrchestrationException);
            }
        }

        private TerminologyMetadataOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyMetadataOrchestrationValidationException =
                new TerminologyMetadataOrchestrationValidationException(
                    message: "Terminology metadata orchestration validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyMetadataOrchestrationValidationException);

            return terminologyMetadataOrchestrationValidationException;
        }
    }
}