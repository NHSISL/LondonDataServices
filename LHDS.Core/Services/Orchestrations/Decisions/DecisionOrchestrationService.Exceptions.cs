// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
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
            catch (DecisionValidationException decisionValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decisionValidationException);
            }
            catch (DocumentValidationException documentValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(documentValidationException);
            }
            catch (DecisionPollValidationException decisionPollValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(decisionPollValidationException);
            }
            catch (DecisionServiceException decisionServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decisionServiceException);
            }
            catch (DocumentDependencyException documentDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentDependencyException);
            }
            catch (DocumentServiceException documentServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(documentServiceException);
            }
            catch (DecisionPollDependencyException decisionPollDependencyException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decisionPollDependencyException);
            }
            catch (DecisionPollServiceException decisionPollServiceException)
            {
                throw await CreateAndLogDependencyExceptionAsync(decisionPollServiceException);
            }
            catch (Exception exception)
            {
                var failedDecryptServiceException =
                    new FailedDecisionOrchestrationServiceException(
                        message: "Failed decision orchestration service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedDecryptServiceException);
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

        private async ValueTask<DecisionOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var decryptionOrchestrationDependencyValidationException =
                new DecisionOrchestrationDependencyValidationException(
                    message:
                        "Decision orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decryptionOrchestrationDependencyValidationException);

            return decryptionOrchestrationDependencyValidationException;
        }

        private async ValueTask<DecisionOrchestrationDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var decisionOrchestrationDependencyException =
                new DecisionOrchestrationDependencyException(
                    message: "Decision orchestration dependency error occurred, fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(decisionOrchestrationDependencyException);

            return decisionOrchestrationDependencyException;
        }

        private async ValueTask<DecisionOrchestrationServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var decisionServiceException =
                new DecisionOrchestrationServiceException(
                    message: "Decision orchestration service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionServiceException);

            return decisionServiceException;
        }
    }
}
