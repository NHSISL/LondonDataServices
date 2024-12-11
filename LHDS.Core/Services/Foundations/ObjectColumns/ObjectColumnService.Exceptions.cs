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
        private delegate IQueryable<ObjectColumn> ReturningObjectColumnsFunction();

        private async ValueTask<ObjectColumn> TryCatch(ReturningObjectColumnFunction returningObjectColumnFunction)
        {
            try
            {
                return await returningObjectColumnFunction();
            }
            catch (NullObjectColumnException nullObjectColumnException)
            {
                throw CreateAndLogValidationException(nullObjectColumnException);
            }
            catch (InvalidObjectColumnException invalidObjectColumnException)
            {
                throw CreateAndLogValidationException(invalidObjectColumnException);
            }
            catch (SqlException sqlException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedObjectColumnStorageException);
            }
            catch (NotFoundObjectColumnException notFoundObjectColumnException)
            {
                throw CreateAndLogValidationException(notFoundObjectColumnException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsObjectColumnException =
                    new AlreadyExistsObjectColumnException(
                        message: "ObjectColumn with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsObjectColumnException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidObjectColumnReferenceException =
                    new InvalidObjectColumnReferenceException(
                        message: "Invalid objectColumn reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidObjectColumnReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedObjectColumnException =
                    new LockedObjectColumnException(
                        message: "Locked objectColumn record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedObjectColumnException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedObjectColumnStorageException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnServiceException =
                    new FailedObjectColumnServiceException(
                        message: "Failed objectColumn service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedObjectColumnServiceException);
            }
        }

        private IQueryable<ObjectColumn> TryCatch(ReturningObjectColumnsFunction returningObjectColumnsFunction)
        {
            try
            {
                return returningObjectColumnsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedObjectColumnStorageException =
                    new FailedObjectColumnStorageException(
                        message: "Failed objectColumn storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedObjectColumnStorageException);
            }
            catch (Exception exception)
            {
                var failedObjectColumnServiceException =
                    new FailedObjectColumnServiceException(
                        message: "Failed objectColumn service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedObjectColumnServiceException);
            }
        }

        private ObjectColumnValidationException CreateAndLogValidationException(Xeption exception)
        {
            var objectColumnValidationException =
                new ObjectColumnValidationException(
                    message: "ObjectColumn validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnValidationException);

            return objectColumnValidationException;
        }

        private ObjectColumnDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var objectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(objectColumnDependencyException);

            return objectColumnDependencyException;
        }

        private ObjectColumnDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var objectColumnDependencyValidationException =
                new ObjectColumnDependencyValidationException(
                    message: "ObjectColumn dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnDependencyValidationException);

            return objectColumnDependencyValidationException;
        }

        private ObjectColumnDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var objectColumnDependencyException =
                new ObjectColumnDependencyException(
                    message: "ObjectColumn dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnDependencyException);

            return objectColumnDependencyException;
        }

        private ObjectColumnServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var objectColumnServiceException =
                new ObjectColumnServiceException(
                    message: "ObjectColumn service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(objectColumnServiceException);

            return objectColumnServiceException;
        }
    }
}