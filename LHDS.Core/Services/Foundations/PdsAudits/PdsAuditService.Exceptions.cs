// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        private delegate IQueryable<PdsAudit> ReturningPdsAuditsFunction();

        private async ValueTask<PdsAudit> TryCatch(ReturningPdsAuditFunction returningPdsAuditFunction)
        {
            try
            {
                return await returningPdsAuditFunction();
            }
            catch (NullPdsAuditException nullPdsAuditException)
            {
                throw CreateAndLogValidationException(nullPdsAuditException);
            }
            catch (InvalidPdsAuditException invalidPdsAuditException)
            {
                throw CreateAndLogValidationException(invalidPdsAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.", 
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedPdsAuditStorageException);
            }
            catch (NotFoundPdsAuditException notFoundPdsAuditException)
            {
                throw CreateAndLogValidationException(notFoundPdsAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPdsAuditException =
                    new AlreadyExistsPdsAuditException(
                        message: "PdsAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPdsAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPdsAuditReferenceException =
                    new InvalidPdsAuditReferenceException(
                        message: "Invalid pdsAudit reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidPdsAuditReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedPdsAuditException = new LockedPdsAuditException(
                    message: "Locked pdsAudit record exception, please try again later", 
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedPdsAuditException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.", 
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedPdsAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedPdsAuditServiceException =
                    new FailedPdsAuditServiceException(
                        message: "Failed pdsAudit service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsAuditServiceException);
            }
        }

        private IQueryable<PdsAudit> TryCatch(ReturningPdsAuditsFunction returningPdsAuditsFunction)
        {
            try
            {
                return returningPdsAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(
                        message: "Failed pdsAudit service error occurred, please contact support.", 
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedPdsAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedPdsAuditServiceException =
                    new FailedPdsAuditServiceException(
                        message: "Failed pdsAudit service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsAuditServiceException);
            }
        }

        private PdsAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsAuditValidationException =
                new PdsAuditValidationException(
                    message: "PdsAudit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(pdsAuditValidationException);

            return pdsAuditValidationException;
        }

        private PdsAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(
                message: "PdsAudit dependency error occurred, please contact support.", 
                innerException: exception);

            this.loggingBroker.LogCritical(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private PdsAuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(
                    message: "PdsAudit dependency validation occurred, please try again.", 
                    innerException: exception);

            this.loggingBroker.LogError(pdsAuditDependencyValidationException);

            return pdsAuditDependencyValidationException;
        }

        private PdsAuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(
                message: "PdsAudit dependency error occurred, please contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private PdsAuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var pdsAuditServiceException = new PdsAuditServiceException(
                message: "PdsAudit service error occurred, please contact support.", 
                innerException: exception);

            this.loggingBroker.LogError(pdsAuditServiceException);

            return pdsAuditServiceException;
        }
    }
}