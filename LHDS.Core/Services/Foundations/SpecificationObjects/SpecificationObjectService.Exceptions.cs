// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using LHDS.Core.Models.Foundations.SpecificationObjects.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SpecificationObjects
{
    public partial class SpecificationObjectService
    {
        private delegate ValueTask<SpecificationObject> ReturningSpecificationObjectFunction();
        private delegate ValueTask<IQueryable<SpecificationObject>> ReturningSpecificationObjectsFunction();

        private async ValueTask<SpecificationObject> TryCatch(ReturningSpecificationObjectFunction returningSpecificationObjectFunction)
        {
            try
            {
                return await returningSpecificationObjectFunction();
            }
            catch (NullSpecificationObjectException nullSpecificationObjectException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSpecificationObjectException);
            }
            catch (InvalidSpecificationObjectException invalidSpecificationObjectException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSpecificationObjectException);
            }
            catch (SqlException sqlException)
            {
                var failedSpecificationObjectStorageException =
                    new FailedSpecificationObjectStorageException(
                        message: "Failed specificationObject storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSpecificationObjectStorageException);
            }
            catch (NotFoundSpecificationObjectException notFoundSpecificationObjectException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundSpecificationObjectException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSpecificationObjectException =
                    new AlreadyExistsSpecificationObjectException(
                        message: "SpecificationObject with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsSpecificationObjectException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSpecificationObjectReferenceException =
                    new InvalidSpecificationObjectReferenceException(
                        message: "Invalid specificationObject reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidSpecificationObjectReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSpecificationObjectException =
                    new LockedSpecificationObjectException(
                        message: "Locked specificationObject record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedSpecificationObjectException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSpecificationObjectStorageException =
                    new FailedSpecificationObjectStorageException(
                        message: "Failed specificationObject storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedSpecificationObjectStorageException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectServiceException =
                    new FailedSpecificationObjectServiceException(
                        message: "Failed specificationObject service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSpecificationObjectServiceException);
            }
        }

        private async ValueTask<IQueryable<SpecificationObject>>
            TryCatch(ReturningSpecificationObjectsFunction returningSpecificationObjectsFunction)
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

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSpecificationObjectStorageException);
            }
            catch (Exception exception)
            {
                var failedSpecificationObjectServiceException =
                    new FailedSpecificationObjectServiceException(
                        message: "Failed specificationObject service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSpecificationObjectServiceException);
            }
        }

        private async ValueTask<SpecificationObjectValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var specificationObjectValidationException =
                new SpecificationObjectValidationException(
                    message: "SpecificationObject validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(specificationObjectValidationException);

            return specificationObjectValidationException;
        }

        private async ValueTask<SpecificationObjectDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var specificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(specificationObjectDependencyException);

            return specificationObjectDependencyException;
        }

        private async ValueTask<SpecificationObjectDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var specificationObjectDependencyValidationException =
                new SpecificationObjectDependencyValidationException(
                    message: "SpecificationObject dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(specificationObjectDependencyValidationException);

            return specificationObjectDependencyValidationException;
        }

        private async ValueTask<SpecificationObjectDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var specificationObjectDependencyException =
                new SpecificationObjectDependencyException(
                    message: "SpecificationObject dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(specificationObjectDependencyException);

            return specificationObjectDependencyException;
        }

        private async ValueTask<SpecificationObjectServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var specificationObjectServiceException =
                new SpecificationObjectServiceException(
                    message: "SpecificationObject service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(specificationObjectServiceException);

            return specificationObjectServiceException;
        }
    }
}