// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using LHDS.Core.Models.Foundations.ObjectColumns.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnService
    {
        private delegate ValueTask<ObjectColumn> ReturningObjectColumnFunction();
        private delegate ValueTask<IQueryable<ObjectColumn>> ReturningObjectColumnsFunction();

        private async ValueTask<ObjectColumn> TryCatch(ReturningObjectColumnFunction returningObjectColumnFunction)
        {
            try
            {
                return await returningObjectColumnFunction();
            }
            catch (NullObjectColumnException nullObjectColumnException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullObjectColumnException);
            }
            catch (InvalidObjectColumnException invalidObjectColumnException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidObjectColumnException);
            }
            catch (SqlException sqlException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedObjectColumnStorageException);
            }
            catch (NotFoundObjectColumnException notFoundObjectColumnException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundObjectColumnException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsObjectColumnException =
                    new AlreadyExistsObjectColumnException(
                        message: "ObjectColumn with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsObjectColumnException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidObjectColumnReferenceException =
                    new InvalidObjectColumnReferenceException(
                        message: "Invalid objectColumn reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidObjectColumnReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedObjectColumnException =
                    new LockedObjectColumnException(
                        message: "Locked objectColumn record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedObjectColumnException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedObjectColumnStorageException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnServiceException =
                    new FailedObjectColumnServiceException(
                        message: "Failed objectColumn service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedObjectColumnServiceException);
            }
        }

        private async ValueTask<IQueryable<ObjectColumn>> TryCatch(
            ReturningObjectColumnsFunction returningObjectColumnsFunction)
        {
            try
            {
                return await returningObjectColumnsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedObjectColumnStorageException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnServiceException =
                    new FailedObjectColumnServiceException(
                        message: "Failed objectColumn service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedObjectColumnServiceException);
            }
        }

        private async ValueTask<ObjectColumnValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var objectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnValidationException);

            return objectColumnValidationException;
        }

        private async ValueTask<ObjectColumnDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var objectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(objectColumnDependencyException);

            return objectColumnDependencyException;
        }

        private async ValueTask<ObjectColumnDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var objectColumnDependencyValidationException =
                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnDependencyValidationException);

            return objectColumnDependencyValidationException;
        }

        private async ValueTask<ObjectColumnDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var objectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnDependencyException);

            return objectColumnDependencyException;
        }

        private async ValueTask<ObjectColumnServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var objectColumnServiceException =
                new ObjectColumnServiceException(
                    message: "ObjectColumn service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(objectColumnServiceException);

            return objectColumnServiceException;
        }
    }
}