// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService
    {
        private delegate ValueTask<Audit> ReturningAuditFunction();
        private delegate IQueryable<Audit> ReturningAuditsFunction();

        private async ValueTask<Audit> TryCatch(ReturningAuditFunction returningAuditFunction)
        {
            try
            {
                return await returningAuditFunction();
            }
            catch (NullAuditException nullAuditException)
            {
                throw CreateAndLogValidationException(nullAuditException);
            }
            catch (InvalidAuditException invalidAuditException)
            {
                throw CreateAndLogValidationException(invalidAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedAuditStorageException =
                    new FailedAuditStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAuditStorageException);
            }
            catch (NotFoundAuditException notFoundAuditException)
            {
                throw CreateAndLogValidationException(notFoundAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAuditException =
                    new AlreadyExistsAuditException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAuditReferenceException =
                    new InvalidAuditReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAuditReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAuditException = new LockedAuditException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAuditException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAuditStorageException =
                    new FailedAuditStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedAuditServiceException =
                    new FailedAuditServiceException(exception);

                throw CreateAndLogServiceException(failedAuditServiceException);
            }
        }

        private IQueryable<Audit> TryCatch(ReturningAuditsFunction returningAuditsFunction)
        {
            try
            {
                return returningAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAuditStorageException =
                    new FailedAuditStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedAuditServiceException =
                    new FailedAuditServiceException(exception);

                throw CreateAndLogServiceException(failedAuditServiceException);
            }
        }

        private AuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var auditValidationException = new AuditValidationException(exception);
            this.loggingBroker.LogError(auditValidationException);

            return auditValidationException;
        }

        private AuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var auditDependencyException = new AuditDependencyException(exception);
            this.loggingBroker.LogCritical(auditDependencyException);

            return auditDependencyException;
        }

        private AuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var auditDependencyValidationException =
                new AuditDependencyValidationException(exception);

            this.loggingBroker.LogError(auditDependencyValidationException);

            return auditDependencyValidationException;
        }

        private AuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var auditDependencyException = new AuditDependencyException(exception);
            this.loggingBroker.LogError(auditDependencyException);

            return auditDependencyException;
        }

        private AuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var auditServiceException = new AuditServiceException(exception);
            this.loggingBroker.LogError(auditServiceException);

            return auditServiceException;
        }
    }
}