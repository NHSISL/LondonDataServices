using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService
    {
        private delegate ValueTask<Audit> ReturningAuditFunction();

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
                    new FailedAuditStorageException(
                        message: "Failed audit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAuditStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAuditException =
                    new AlreadyExistsAuditException(
                        message: "Audit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAuditReferenceException =
                    new InvalidAuditReferenceException(
                        message: "Invalid audit reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAuditReferenceException);
            }
        }

        private AuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var auditValidationException =
                new AuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(auditValidationException);

            return auditValidationException;
        }

        private AuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var auditDependencyException = 
                new AuditDependencyException(
                    message: "Audit dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(auditDependencyException);

            return auditDependencyException;
        }

        private AuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var auditDependencyValidationException =
                new AuditDependencyValidationException(
                    message: "Audit dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(auditDependencyValidationException);

            return auditDependencyValidationException;
        }
    }
}