// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationService
    {
        private delegate ValueTask<List<Decision>> ReturningDecisionsFunction();

        private async ValueTask<List<Decision>> TryCatch(ReturningDecisionsFunction returningDecisionsFunction)
        {
            try
            {
                return await returningDecisionsFunction();
            }
            catch (NullBlobContainersDecisionOrchestrationException nullBlobContainersDecisionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullBlobContainersDecisionOrchestrationException);
            }
            catch (InvalidDecisionPollsDecisionOrchestrationException
                invalidDecisionPollsDecisionOrchestrationException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDecisionPollsDecisionOrchestrationException);
            }
        }

        private async ValueTask<DecisionOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var decisionOrchestrationValidationException =
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionOrchestrationValidationException);

            return decisionOrchestrationValidationException;
        }
    }
}
