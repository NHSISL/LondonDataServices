// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private delegate ValueTask<PdsAudit> ReturningPdsAuditFunction();
        private delegate ValueTask<IQueryable<PdsAudit>> ReturningPdsAuditsFunction();

        private async ValueTask<PdsAudit> TryCatch(ReturningPdsAuditFunction returningPdsAuditFunction)
        {
            try
            {
                return await returningPdsAuditFunction();
            }
            catch (NullPdsAuditException nullPdsAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullPdsAuditException);
            }
            catch (InvalidPdsAuditException invalidPdsAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidPdsAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedPdsAuditStorageException);
            }
            catch (NotFoundPdsAuditException notFoundPdsAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundPdsAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPdsAuditException =
                    new AlreadyExistsPdsAuditException(
                        message: "PdsAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsPdsAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPdsAuditReferenceException =
                    new InvalidPdsAuditReferenceException(
                        message: "Invalid pdsAudit reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidPdsAuditReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPdsAuditException = new LockedPdsAuditException(
                    message: "Locked pdsAudit record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedPdsAuditException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedPdsAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedPdsAuditServiceException =
                    new FailedPdsAuditServiceException(
                        message: "Failed pdsAudit service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedPdsAuditServiceException);
            }
        }

        private async ValueTask<IQueryable<PdsAudit>> TryCatch(ReturningPdsAuditsFunction returningPdsAuditsFunction)
        {
            try
            {
                return await returningPdsAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedPdsAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedPdsAuditServiceException =
                    new FailedPdsAuditServiceException(
                        message: "Failed pdsAudit service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedPdsAuditServiceException);
            }
        }

        private async ValueTask<PdsAuditValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var pdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsAuditValidationException);

            return pdsAuditValidationException;
        }

        private async ValueTask<PdsAuditDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(
                message: "PdsAudit dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private async ValueTask<PdsAuditDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: "PdsAudit dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsAuditDependencyValidationException);

            return pdsAuditDependencyValidationException;
        }

        private async ValueTask<PdsAuditDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(
                message: "PdsAudit dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private async ValueTask<PdsAuditServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var pdsAuditServiceException = new PdsAuditServiceException(
                message: "PdsAudit service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(pdsAuditServiceException);

            return pdsAuditServiceException;
        }
    }
}