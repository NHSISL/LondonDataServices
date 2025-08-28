// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<T> ReturningIngestionTrackingFunction<T>();
        private delegate IQueryable<IngestionTracking> ReturningIngestionTrackingsFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullIngestionTrackingException nullIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingException);
            }
            catch (InvalidIngestionTrackingException invalidIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidIngestionTrackingException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedIngestionTrackingStorageException);
            }
            catch (NotFoundIngestionTrackingException notFoundIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundIngestionTrackingException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(
                        message: "Ingestion tracking with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(
                        message: "Invalid ingestion tracking reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogCriticalDependencyValidationExceptionAsync(
                    invalidIngestionTrackingReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedIngestionTrackingException = new LockedIngestionTrackingException(
                    message: "Locked ingestion tracking record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedIngestionTrackingException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(
                        message: "Failed ingestion tracking service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingServiceException);
            }
        }

        private async ValueTask<T> TryCatch<T>(ReturningIngestionTrackingFunction<T> returningIngestionTrackingFunction)
        {
            try
            {
                return await returningIngestionTrackingFunction();
            }
            catch (NullIngestionTrackingException nullIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIngestionTrackingException);
            }
            catch (InvalidIngestionTrackingException invalidIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidIngestionTrackingException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedIngestionTrackingStorageException);
            }
            catch (NotFoundIngestionTrackingException notFoundIngestionTrackingException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundIngestionTrackingException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(
                        message: "Ingestion tracking with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(
                        message: "Invalid ingestion tracking reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidIngestionTrackingReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedIngestionTrackingException = new LockedIngestionTrackingException(
                    message: "Locked ingestion tracking record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedIngestionTrackingException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(
                        message: "Failed ingestion tracking service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingServiceException);
            }
        }

        private async Task<IQueryable<IngestionTracking>> TryCatch(ReturningIngestionTrackingsFunction returningIngestionTrackingsFunction)
        {
            try
            {
                return returningIngestionTrackingsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(
                        message: "Failed ingestion tracking service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedIngestionTrackingServiceException);
            }
        }

        private async ValueTask<IngestionTrackingValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingValidationException);

            return ingestionTrackingValidationException;
        }

        private async ValueTask<IngestionTrackingDependencyException> CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(
                message: "Failed ingestion tracking storage error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private async ValueTask<IngestionTrackingDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingDependencyValidationException);

            return ingestionTrackingDependencyValidationException;
        }

        private async ValueTask<IngestionTrackingDependencyValidationException> CreateAndLogCriticalDependencyValidationExceptionAsync(Xeption exception)
        {
            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(ingestionTrackingDependencyValidationException);

            return ingestionTrackingDependencyValidationException;
        }

        private async ValueTask<IngestionTrackingDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(
                message: "Failed ingestion tracking storage error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private async ValueTask<IngestionTrackingServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var ingestionTrackingServiceException = new IngestionTrackingServiceException(
                message: "Ingestion tracking service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(ingestionTrackingServiceException);

            return ingestionTrackingServiceException;
        }
    }
}