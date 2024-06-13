using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService
    {
        private delegate ValueTask<TerminologyPoll> ReturningTerminologyPollFunction();
        private delegate IQueryable<TerminologyPoll> ReturningTerminologyPollsFunction();

        private async ValueTask<TerminologyPoll> TryCatch(ReturningTerminologyPollFunction returningTerminologyPollFunction)
        {
            try
            {
                return await returningTerminologyPollFunction();
            }
            catch (NullTerminologyPollException nullTerminologyPollException)
            {
                throw CreateAndLogValidationException(nullTerminologyPollException);
            }
            catch (InvalidTerminologyPollException invalidTerminologyPollException)
            {
                throw CreateAndLogValidationException(invalidTerminologyPollException);
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyPollStorageException);
            }
            catch (NotFoundTerminologyPollException notFoundTerminologyPollException)
            {
                throw CreateAndLogValidationException(notFoundTerminologyPollException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTerminologyPollException =
                    new AlreadyExistsTerminologyPollException(
                        message: "TerminologyPoll with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsTerminologyPollException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTerminologyPollReferenceException =
                    new InvalidTerminologyPollReferenceException(
                        message: "Invalid terminologyPoll reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidTerminologyPollReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTerminologyPollException = 
                    new LockedTerminologyPollException(
                        message: "Locked terminologyPoll record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedTerminologyPollException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedTerminologyPollStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollServiceException =
                    new FailedTerminologyPollServiceException(
                        message: "Failed terminologyPoll service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyPollServiceException);
            }
        }

        private IQueryable<TerminologyPoll> TryCatch(ReturningTerminologyPollsFunction returningTerminologyPollsFunction)
        {
            try
            {
                return returningTerminologyPollsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyPollStorageException =
                    new FailedTerminologyPollStorageException(
                        message: "Failed terminologyPoll storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyPollStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyPollServiceException =
                    new FailedTerminologyPollServiceException(
                        message: "Failed terminologyPoll service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyPollServiceException);
            }
        }

        private TerminologyPollValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollValidationException);

            return terminologyPollValidationException;
        }

        private TerminologyPollDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var terminologyPollDependencyException = 
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(terminologyPollDependencyException);

            return terminologyPollDependencyException;
        }

        private TerminologyPollDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var terminologyPollDependencyValidationException =
                new TerminologyPollDependencyValidationException(
                    message: "TerminologyPoll dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollDependencyValidationException);

            return terminologyPollDependencyValidationException;
        }

        private TerminologyPollDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var terminologyPollDependencyException = 
                new TerminologyPollDependencyException(
                    message: "TerminologyPoll dependency error occurred, please contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(terminologyPollDependencyException);

            return terminologyPollDependencyException;
        }

        private TerminologyPollServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var terminologyPollServiceException = 
                new TerminologyPollServiceException(
                    message: "TerminologyPoll service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyPollServiceException);

            return terminologyPollServiceException;
        }
    }
}