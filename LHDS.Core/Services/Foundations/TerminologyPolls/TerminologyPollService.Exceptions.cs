// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService
    {
        private delegate ValueTask<TerminologyPoll> ReturningTerminologyPollFunction();
        private delegate ValueTask<IQueryable<TerminologyPoll>> ReturningTerminologyPollsFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollException nullTerminologyPollException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullTerminologyPollException);
            }
            catch (InvalidTerminologyPollException invalidTerminologyPollException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidTerminologyPollException);
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedTerminologyPollStorageException);
            }
            catch (NotFoundTerminologyPollException notFoundTerminologyPollException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundTerminologyPollException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTerminologyPollException =
                    new AlreadyExistsTerminologyPollException(
                        message: "TerminologyPoll with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsTerminologyPollException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTerminologyPollReferenceException =
                    new InvalidTerminologyPollReferenceException(
                        message: "Invalid terminologyPoll reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidTerminologyPollReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTerminologyPollException =
                    new LockedTerminologyPollException(
                        message: "Locked terminologyPoll record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedTerminologyPollException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedTerminologyPollStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollServiceException =
                    new FailedTerminologyPollServiceException(
                        message: "Failed terminologyPoll service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyPollServiceException);
            }
        }

        private async ValueTask<IQueryable<TerminologyPoll>>
            TryCatch(ReturningTerminologyPollsFunction returningTerminologyPollsFunction)
        {
            try
            {
                return await returningTerminologyPollsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedTerminologyPollStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollServiceException =
                    new FailedTerminologyPollServiceException(
                        message: "Failed terminologyPoll service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedTerminologyPollServiceException);
            }
        }

        private async ValueTask<TerminologyPollValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var terminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollValidationException);

            return terminologyPollValidationException;
        }

        private async ValueTask<TerminologyPollDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(terminologyPollDependencyException);

            return terminologyPollDependencyException;
        }

        private async  ValueTask<TerminologyPollDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollDependencyValidationException);

            return terminologyPollDependencyValidationException;
        }

        private async ValueTask<TerminologyPollDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyPollDependencyException =
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollDependencyException);

            return terminologyPollDependencyException;
        }

        private async ValueTask<TerminologyPollServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var terminologyPollServiceException =
                new TerminologyPollServiceException(
                    message: "TerminologyPoll service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyPollServiceException);

            return terminologyPollServiceException;
        }
    }
}