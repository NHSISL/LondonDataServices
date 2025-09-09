// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Decisions
{
    public partial class DecisionService
    {
        private delegate ValueTask<List<Decision>> ReturningDecisionsFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<List<Decision>> TryCatch(ReturningDecisionsFunction returningDecisionsFunction)
        {
            try
            {
                return await returningDecisionsFunction();
            }
            catch (InvalidDecisionsException invalidDecisionsException)
            {
                throw await CreateAndLogValidationException(invalidDecisionsException);
            }
            catch (Exception exception)
            {
                var failedDecisionServiceException =
                    new FailedDecisionServiceException(
                        message: "Failed decision service occurred, please contact support",
                        innerException: exception);

                throw await CreateAndLogServiceException(failedDecisionServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returbingNothingFunction)
        {
            try
            {
                await returbingNothingFunction();
            }
            catch (NullDecisionsException nullDecisionsException)
            {
                throw await CreateAndLogValidationException(nullDecisionsException);
            }
            catch (InvalidDecisionsException invalidDecisionsException)
            {
                throw await CreateAndLogValidationException(invalidDecisionsException);
            }
            catch (Exception exception)
            {
                var failedDecisionServiceException =
                    new FailedDecisionServiceException(
                        message: "Failed decision service occurred, please contact support",
                        innerException: exception);

                throw await CreateAndLogServiceException(failedDecisionServiceException);
            }
        }

        private async ValueTask<DecisionValidationException> CreateAndLogValidationException(Xeption exception)
        {
            var decisionValidationException =
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionValidationException);

            return decisionValidationException;
        }

        private async ValueTask<DecisionServiceException> CreateAndLogServiceException(
            Xeption exception)
        {
            var decisionServiceException =
                new DecisionServiceException(
                    message: "Decision service error occurred, contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionServiceException);

            return decisionServiceException;
        }
    }
}
