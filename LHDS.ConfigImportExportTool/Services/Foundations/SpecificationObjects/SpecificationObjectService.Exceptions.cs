// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using EFxceptions.Models.Exceptions;
using LHDS.ConfigImportExportTool.Models.Foundations.SpecificationObjects;
using LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.SpecificationObjects
{
    internal partial class SpecificationObjectService
    {
        private delegate ValueTask<SpecificationObject> ReturningSpecificationObjectFunction();
        private delegate ValueTask<IQueryable<SpecificationObject>> ReturningSpecificationObjectsFunction();

        private async ValueTask<SpecificationObject> TryCatch(
            ReturningSpecificationObjectFunction returningSpecificationObjectFunction)
        {
            try
            {
                return await returningSpecificationObjectFunction();
            }
            catch (NullSpecificationObjectException nullSpecificationObjectException)
            {
                throw CreateAndLogValidationException(nullSpecificationObjectException);
            }
            catch (InvalidSpecificationObjectException invalidSpecificationObjectException)
            {
                throw CreateAndLogValidationException(invalidSpecificationObjectException);
            }
            catch (SqlException sqlException)
            {
                var failedSpecificationObjectStorageException =
                    new FailedSpecificationObjectStorageException(
                        message: "Failed specificationObject storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSpecificationObjectStorageException);
            }
            catch (NotFoundSpecificationObjectException notFoundSpecificationObjectException)
            {
                throw CreateAndLogValidationException(notFoundSpecificationObjectException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSpecificationObjectException =
                    new AlreadyExistsSpecificationObjectException(
                        message: "SpecificationObject with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSpecificationObjectException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSpecificationObjectReferenceException =
                    new InvalidSpecificationObjectReferenceException(
                        message: "Invalid specificationObject reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidSpecificationObjectReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSpecificationObjectException =
                    new LockedSpecificationObjectException(
                        message: "Locked specificationObject record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedSpecificationObjectException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSpecificationObjectStorageException =
                    new FailedSpecificationObjectStorageException(
                        message: "Failed specificationObject storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedSpecificationObjectStorageException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectServiceException =
                    new FailedSpecificationObjectServiceException(
                        message: "Failed specificationObject service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSpecificationObjectServiceException);
            }
        }

        private async ValueTask<IQueryable<SpecificationObject>> TryCatch(
            ReturningSpecificationObjectsFunction returningSpecificationObjectsFunction)
        {
            try
            {
                return await returningSpecificationObjectsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSpecificationObjectStorageException =
                    new FailedSpecificationObjectStorageException(
                        message: "Failed specificationObject storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSpecificationObjectStorageException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectServiceException =
                    new FailedSpecificationObjectServiceException(
                        message: "Failed specificationObject service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSpecificationObjectServiceException);
            }
        }

        private SpecificationObjectValidationException CreateAndLogValidationException(Xeption exception)
        {
            var specificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectValidationException);

            return specificationObjectValidationException;
        }

        private SpecificationObjectDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var specificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCriticalAsync(specificationObjectDependencyException);

            return specificationObjectDependencyException;
        }

        private SpecificationObjectDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var specificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectDependencyValidationException);

            return specificationObjectDependencyValidationException;
        }

        private SpecificationObjectDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var specificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectDependencyException);

            return specificationObjectDependencyException;
        }

        private SpecificationObjectServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var specificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(specificationObjectServiceException);

            return specificationObjectServiceException;
        }
    }
}