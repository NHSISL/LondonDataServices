// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollService
    {
        private delegate ValueTask<DecisionPoll> ReturningDecisionPollFunction();

        private async ValueTask<DecisionPoll> TryCatch(ReturningDecisionPollFunction returningDecisionPollFunction)
        {
            try
            {
                return await returningDecisionPollFunction();
            }
            catch (NullDecisionPollException nullDecisionPollException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDecisionPollException);
            }
        }

        private async ValueTask<DecisionPollValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var decisionPollValidationException =
                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionPollValidationException);

            return decisionPollValidationException;
        }
    }
}
