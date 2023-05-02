using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private delegate ValueTask<PdsAudit> ReturningPdsAuditFunction();

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
                    new FailedPdsAuditStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPdsAuditStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsPdsAuditException =
                    new AlreadyExistsPdsAuditException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsPdsAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidPdsAuditReferenceException =
                    new InvalidPdsAuditReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidPdsAuditReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedPdsAuditStorageException =
                    new FailedPdsAuditStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedPdsAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedPdsAuditServiceException =
                    new FailedPdsAuditServiceException(exception);

                throw CreateAndLogServiceException(failedPdsAuditServiceException);
            }
        }

        private PdsAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsAuditValidationException =
                new PdsAuditValidationException(exception);

            this.loggingBroker.LogError(pdsAuditValidationException);

            return pdsAuditValidationException;
        }

        private PdsAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(exception);
            this.loggingBroker.LogCritical(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private PdsAuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var pdsAuditDependencyValidationException =
                new PdsAuditDependencyValidationException(exception);

            this.loggingBroker.LogError(pdsAuditDependencyValidationException);

            return pdsAuditDependencyValidationException;
        }

        private PdsAuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var pdsAuditDependencyException = new PdsAuditDependencyException(exception);
            this.loggingBroker.LogError(pdsAuditDependencyException);

            return pdsAuditDependencyException;
        }

        private PdsAuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var pdsAuditServiceException = new PdsAuditServiceException(exception);
            this.loggingBroker.LogError(pdsAuditServiceException);

            return pdsAuditServiceException;
        }
    }
}