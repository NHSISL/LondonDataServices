// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationService
    {
        private void ValidateBlobContainersIsNotNull()
        {
            if (this.blobContainers is null)
            {
                throw new NullBlobContainersDecisionOrchestrationException(
                    message: "Null blob container decision orchestration exception, " +
                             "please correct the errors and try again.");
            }
        }
    }
}
