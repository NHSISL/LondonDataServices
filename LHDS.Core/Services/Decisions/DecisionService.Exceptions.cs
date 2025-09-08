// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Decisions
{
    public partial class DecisionService
    {
        private delegate ValueTask<List<Decision>> ReturningDecisionsFunction();

        private async ValueTask<List<Decision>> TryCatch(ReturningDecisionsFunction returningDecisionsFunction)
        {
            try
            {
                return await returningDecisionsFunction();
            }
            catch (NullDecisionsException nullDecisionsException)
            {
                throw await CreateAndLogValidationException(nullDecisionsException);
            }
            catch (Exception exception)
            {
                throw exception;
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
    }
}
