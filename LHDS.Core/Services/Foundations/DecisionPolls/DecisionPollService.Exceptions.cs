// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (InvalidDecisionPollException invalidDecisionPollException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDecisionPollException);
            }
            catch (SqlException sqlException)
            {
                var failedDecisionPollStorageException =
                    new FailedDecisionPollStorageException(
                        message: "Failed decisionPoll storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedDecisionPollStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDecisionPollException =
                    new AlreadyExistsDecisionPollException(
                        message: "DecisionPoll with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsDecisionPollException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidDecisionPollReferenceException =
                    new InvalidDecisionPollReferenceException(
                        message: "Invalid decisionPoll reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDecisionPollReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedDecisionPollStorageException =
                    new FailedDecisionPollStorageException(
                        message: "Failed decisionPoll storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedDecisionPollStorageException);
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

        private async ValueTask<DecisionPollDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var decisionPollDependencyException =
                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(decisionPollDependencyException);

            return decisionPollDependencyException;
        }

        private async ValueTask<DecisionPollDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var decisionPollDependencyValidationException =
                new DecisionPollDependencyValidationException(
                    message: "DecisionPoll dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionPollDependencyValidationException);

            return decisionPollDependencyValidationException;
        }

        private async ValueTask<DecisionPollDependencyException> CreateAndLogDependencyExceptionAsync(Xeption exception)
        {
            var decisionPollDependencyException =
                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(decisionPollDependencyException);

            return decisionPollDependencyException;
        }
    }
}
