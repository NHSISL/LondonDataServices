// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Landings.Client.Models.IngestionTracking;
using LHDS.Landings.Client.Models.IngestionTracking.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private delegate ValueTask<IngestionTracking> ReturningIngestionTrackingFunction();

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
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingStorageException =
                    new FailedIngestionTrackingStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingException =
                    new AlreadyExistsIngestionTrackingException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsIngestionTrackingException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingReferenceException =
                    new InvalidIngestionTrackingReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidIngestionTrackingReferenceException);
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

    }
}
