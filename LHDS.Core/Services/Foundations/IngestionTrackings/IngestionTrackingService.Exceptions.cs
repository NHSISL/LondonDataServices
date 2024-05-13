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
        private delegate ValueTask<T> ReturningIngestionTrackingFunction<T>();
        private delegate IQueryable<IngestionTracking> ReturningIngestionTrackingsFunction();

        private async ValueTask<T> TryCatch<T>(ReturningIngestionTrackingFunction<T> returningIngestionTrackingFunction)
        {
            try
            {
                return await returningIngestionTrackingFunction();
            }
            catch (NullIngestionTrackingException nullIngestionTrackingException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingException);
            }
            catch (InvalidIngestionTrackingException invalidIngestionTrackingException)
            {
                throw CreateAndLogValidationException(invalidIngestionTrackingException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (NotFoundIngestionTrackingException notFoundIngestionTrackingException)
            {
                throw CreateAndLogValidationException(notFoundIngestionTrackingException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(
                        message: "Ingestion tracking with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(
                        message: "Invalid ingestion tracking reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidIngestionTrackingReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedIngestionTrackingException = new LockedIngestionTrackingException(
                    message: "Locked ingestion tracking record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedIngestionTrackingException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(
                        message: "Failed ingestion tracking storage error occurred, please contact support.",
                        databaseUpdateException);

                throw CreateAndLogDependencyException(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(
                        message: "Failed ingestion tracking service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingServiceException);
            }
        }

        private IQueryable<IngestionTracking> TryCatch(ReturningIngestionTrackingsFunction returningIngestionTrackingsFunction)
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

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(
                        message: "Failed ingestion tracking service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingServiceException);
            }
        }

        private IngestionTrackingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingValidationException);

            return ingestionTrackingValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(
                message: "Failed ingestion tracking storage error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogCritical(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private IngestionTrackingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingDependencyValidationException);

            return ingestionTrackingDependencyValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(
                message: "Failed ingestion tracking storage error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private IngestionTrackingServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ingestionTrackingServiceException = new IngestionTrackingServiceException(
                message: "Ingestion tracking service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(ingestionTrackingServiceException);

            return ingestionTrackingServiceException;
        }
    }
}