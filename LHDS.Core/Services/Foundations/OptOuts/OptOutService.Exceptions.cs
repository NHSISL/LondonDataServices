// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Foundations.OptOuts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OptOuts
{
    public partial class OptOutService
    {
        private delegate ValueTask<OptOut> ReturningOptOutFunction();
        private delegate ValueTask<IQueryable<OptOut>> ReturningOptOutsFunction();

        private async ValueTask<OptOut> TryCatch(ReturningOptOutFunction returningOptOutFunction)
        {
            try
            {
                return await returningOptOutFunction();
            }
            catch (NullOptOutException nullOptOutException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullOptOutException);
            }
            catch (InvalidOptOutException invalidOptOutException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidOptOutException);
            }
            catch (SqlException sqlException)
            {
                var failedOptOutStorageException = new FailedOptOutStorageException(
                    message: "Failed optOut storage error occurred, please contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedOptOutStorageException);
            }
            catch (NotFoundOptOutException notFoundOptOutException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundOptOutException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOptOutException =
                    new AlreadyExistsOptOutException(
                        message: "OptOut with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsOptOutException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidOptOutReferenceException = new InvalidOptOutReferenceException(
                    message: "Invalid optOut reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidOptOutReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedOptOutException = new LockedOptOutException(
                    message: "Locked optOut record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedOptOutException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOptOutStorageException = new FailedOptOutStorageException(
                    message: "Failed optOut storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedOptOutStorageException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException = new FailedOptOutServiceException(
                    message: "Failed optOut service error occurred, please contact support.",
                    innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutServiceException);
            }
        }

        private async ValueTask<IQueryable<OptOut>> TryCatch(ReturningOptOutsFunction returningOptOutsFunction)
        {
            try
            {
                return await returningOptOutsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedOptOutStorageException = new FailedOptOutStorageException(
                    message: "Failed optOut storage error occurred, please contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedOptOutStorageException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException = new FailedOptOutServiceException(
                    message: "Failed optOut service error occurred, please contact support.",
                    innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedOptOutServiceException);
            }
        }

        private async ValueTask<OptOutValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var optOutValidationException = new OptOutValidationException(
                message: "OptOut validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutValidationException);

            return optOutValidationException;
        }

        private async ValueTask<OptOutDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var optOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(optOutDependencyException);

            return optOutDependencyException;
        }

        private async ValueTask<OptOutDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(
            Xeption exception)
        {
            var optOutDependencyValidationException = new OptOutDependencyValidationException(
                message: "OptOut dependency validation occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutDependencyValidationException);

            return optOutDependencyValidationException;
        }

        private async ValueTask<OptOutDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var optOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutDependencyException);

            return optOutDependencyException;
        }

        private async ValueTask<OptOutServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var optOutServiceException = new OptOutServiceException(
                message: "OptOut service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(optOutServiceException);

            return optOutServiceException;
        }
    }
}