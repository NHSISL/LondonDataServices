// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using LHDS.Core.Models.Foundations.DecisionPolls;
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

        private void ValidateDecisionConfigurationIsNotNull()
        {
            if (this.decisionConfiguration is null)
            {
                throw new NullDecisionConfigurationDecisionOrchestrationException(
                    message: "Null decision configuration decision orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateDecisionPolls(IQueryable<DecisionPoll> decisionPolls)
        {
            if (decisionPolls is null || !decisionPolls.Any())
            {
                throw new InvalidDecisionPollsDecisionOrchestrationException(message: "DecisionPolls required.");
            }
        }
    }
}
