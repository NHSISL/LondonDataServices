// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Coordinations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Coordinations.Decisions
{
    public partial class DecisionCoordinationService
    {
        private delegate ValueTask<List<Decision>> ReturningDecisionsFunction();

        private async ValueTask<List<Decision>> TryCatch(ReturningDecisionsFunction returningDecisionsFunction)
        {
            try
            {
                return await returningDecisionsFunction();
            }
            catch (DecisionOrchestrationValidationException decisionOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decisionOrchestrationValidationException);
            }
            catch (DecisionOrchestrationDependencyValidationException
                   decisionOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    decisionOrchestrationDependencyValidationException);
            }
        }

        private async ValueTask<DecisionCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressCoordinationDependencyValidationException =
                new DecisionCoordinationDependencyValidationException(
                    message: "Decision coordination dependency validation error occurred, please try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(addressCoordinationDependencyValidationException);

            return addressCoordinationDependencyValidationException;
        }
    }
}
