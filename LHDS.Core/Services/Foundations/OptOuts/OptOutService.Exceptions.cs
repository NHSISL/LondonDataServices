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
                throw CreateAndLogValidationException(nullOptOutException);
            }
            catch (InvalidOptOutException invalidOptOutException)
            {
                throw CreateAndLogValidationException(invalidOptOutException);
            }
            catch (SqlException sqlException)
            {
                var failedOptOutStorageException = new FailedOptOutStorageException(
                    message: "Failed optOut storage error occurred, please contact support.",
                    innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOptOutStorageException);
            }
            catch (NotFoundOptOutException notFoundOptOutException)
            {
                throw CreateAndLogValidationException(notFoundOptOutException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOptOutException =
                    new AlreadyExistsOptOutException(
                        message: "OptOut with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsOptOutException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidOptOutReferenceException = new InvalidOptOutReferenceException(
                    message: "Invalid optOut reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidOptOutReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedOptOutException = new LockedOptOutException(
                    message: "Locked optOut record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedOptOutException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOptOutStorageException = new FailedOptOutStorageException(
                    message: "Failed optOut storage error occurred, please contact support.",
                    innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedOptOutStorageException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException = new FailedOptOutServiceException(
                    message: "Failed optOut service error occurred, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
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

                throw CreateAndLogCriticalDependencyException(failedOptOutStorageException);
            }
            catch (Exception exception)
            {
                var failedOptOutServiceException = new FailedOptOutServiceException(
                    message: "Failed optOut service error occurred, please contact support.",
                    innerException: exception);

                throw CreateAndLogServiceException(failedOptOutServiceException);
            }
        }

        private OptOutValidationException CreateAndLogValidationException(Xeption exception)
        {
            var optOutValidationException = new OptOutValidationException(
                message: "OptOut validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(optOutValidationException);

            return optOutValidationException;
        }

        private OptOutDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var optOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogCritical(optOutDependencyException);

            return optOutDependencyException;
        }

        private OptOutDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var optOutDependencyValidationException = new OptOutDependencyValidationException(
                message: "OptOut dependency validation occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(optOutDependencyValidationException);

            return optOutDependencyValidationException;
        }

        private OptOutDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var optOutDependencyException = new OptOutDependencyException(
                message: "OptOut dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(optOutDependencyException);

            return optOutDependencyException;
        }

        private OptOutServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var optOutServiceException = new OptOutServiceException(
                message: "OptOut service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(optOutServiceException);

            return optOutServiceException;
        }
    }
}