using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Audits;
using LHDS.Core.Models.Audits.Exceptions;
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
                    new FailedAuditStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAuditStorageException);
            }
        }

        private AuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var auditValidationException =
                new AuditValidationException(exception);

            this.loggingBroker.LogError(auditValidationException);

            return auditValidationException;
        }

        private AuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var auditDependencyException = new AuditDependencyException(exception);
            this.loggingBroker.LogCritical(auditDependencyException);

            return auditDependencyException;
        }
    }
}