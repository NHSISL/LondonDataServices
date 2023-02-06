// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptionss;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private delegate ValueTask<IngestionTracking> ReturningIngestionTrackingFunction();
        private delegate IQueryable<IngestionTracking> ReturningIngestionTrackingsFunction();

        private async ValueTask<IngestionTracking> TryCatch(ReturningIngestionTrackingFunction returningIngestionTrackingFunction)
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsIngestionTrackingException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (NotFoundIngestionTrackingException notFoundIngestionTrackingException)
            {
                throw CreateAndLogValidationException(notFoundIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidIngestionTrackingReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedIngestionTrackingException = new LockedIngestionTrackingException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedIngestionTrackingException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(exception);

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
                    new FailedIngestionTrackingStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingServiceException =
                    new FailedIngestionTrackingServiceException(exception);

                throw CreateAndLogServiceException(failedIngestionTrackingServiceException);
            }
        }

        private IngestionTrackingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingValidationException =
                new IngestionTrackingValidationException(exception);

            this.loggingBroker.LogError(ingestionTrackingValidationException);

            return ingestionTrackingValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(exception);
            this.loggingBroker.LogCritical(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private IngestionTrackingDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ingestionTrackingDependencyValidationException =
                new IngestionTrackingDependencyValidationException(exception);

            this.loggingBroker.LogError(ingestionTrackingDependencyValidationException);

            return ingestionTrackingDependencyValidationException;
        }

        private IngestionTrackingDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ingestionTrackingDependencyException = new IngestionTrackingDependencyException(exception);
            this.loggingBroker.LogError(ingestionTrackingDependencyException);

            return ingestionTrackingDependencyException;
        }

        private IngestionTrackingServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ingestionTrackingServiceException = new IngestionTrackingServiceException(exception);
            this.loggingBroker.LogError(ingestionTrackingServiceException);

            return ingestionTrackingServiceException;
        }
    }
}