// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditService
    {
        private delegate ValueTask<IngestionTrackingAudit> ReturningIngestionTrackingAuditFunction();
        private delegate ValueTask<IQueryable<IngestionTrackingAudit>> ReturningIngestionTrackingAuditsFunction();

        private async ValueTask<IngestionTrackingAudit> TryCatch(
            ReturningIngestionTrackingAuditFunction returningAuditFunction)
        {
            try
            {
                return await returningAuditFunction();
            }
            catch (NullIngestionTrackingAuditException nullIngestionTrackingAuditException)
            {
                throw CreateAndLogValidationException(nullIngestionTrackingAuditException);
            }
            catch (InvalidIngestionTrackingAuditException invalidIngestionTrackingAuditException)
            {
                throw CreateAndLogValidationException(invalidIngestionTrackingAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingAuditStorageException =
                    new FailedIngestionTrackingAuditStorageException(
                        message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingAuditStorageException);
            }
            catch (NotFoundIngestionTrackingAuditException notFoundAuditException)
            {
                throw CreateAndLogValidationException(notFoundAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsIngestionTrackingAuditException =
                    new AlreadyExistsIngestionTrackingAuditException(
                        message: "IngestionTrackingAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsIngestionTrackingAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidIngestionTrackingAuditReferenceException =
                    new InvalidIngestionTrackingAuditReferenceException(
                        message: "Invalid IngestionTrackingAudit reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidIngestionTrackingAuditReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedIngestionTrackingAuditException = new LockedIngestionTrackingAuditException(
                    message: "Locked IngestionTrackingAudit record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedIngestionTrackingAuditException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedIngestionTrackingAuditStorageException =
                    new FailedIngestionTrackingAuditStorageException(
                        message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedIngestionTrackingAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditServiceException =
                    new FailedIngestionTrackingAuditServiceException(
                        message: "Failed IngestionTrackingAudit service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingAuditServiceException);
            }
        }

        private async ValueTask<IQueryable<IngestionTrackingAudit>> TryCatch(
            ReturningIngestionTrackingAuditsFunction returningAuditsFunction)
        {
            try
            {
                return await returningAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedIngestionTrackingAuditStorageException =
                    new FailedIngestionTrackingAuditStorageException(
                        message: "Failed IngestionTrackingAudit storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedIngestionTrackingAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedIngestionTrackingAuditServiceException =
                    new FailedIngestionTrackingAuditServiceException(
                        message: "Failed IngestionTrackingAudit service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedIngestionTrackingAuditServiceException);
            }
        }

        private IngestionTrackingAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ingestionTrackingAuditValidationException = new IngestionTrackingAuditValidationException(
                message: "IngestionTrackingAudit validation errors occurred, please try again.",
                exception);

            this.loggingBroker.LogErrorAsync(ingestionTrackingAuditValidationException);

            return ingestionTrackingAuditValidationException;
        }

        private IngestionTrackingAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ingestionTrackingAuditDependencyException = new IngestionTrackingAuditDependencyException(
                message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                innerException: exception);
            this.loggingBroker.LogCriticalAsync(ingestionTrackingAuditDependencyException);

            return ingestionTrackingAuditDependencyException;
        }

        private IngestionTrackingAuditDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var ingestionTrackingAuditDependencyValidationException =
                new IngestionTrackingAuditDependencyValidationException(
                    message: "IngestionTrackingAudit dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(ingestionTrackingAuditDependencyValidationException);

            return ingestionTrackingAuditDependencyValidationException;
        }

        private IngestionTrackingAuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ingestionTrackingAuditDependencyException = new IngestionTrackingAuditDependencyException(
                message: "IngestionTrackingAudit dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(ingestionTrackingAuditDependencyException);

            return ingestionTrackingAuditDependencyException;
        }

        private IngestionTrackingAuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ingestionTrackingAuditServiceException = new IngestionTrackingAuditServiceException(
                message: "IngestionTrackingAudit service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(ingestionTrackingAuditServiceException);

            return ingestionTrackingAuditServiceException;
        }
    }
}