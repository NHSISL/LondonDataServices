using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditService
    {
        private delegate ValueTask<AddressLoadingAudit> ReturningAddressLoadingAuditFunction();

        private async ValueTask<AddressLoadingAudit> TryCatch(ReturningAddressLoadingAuditFunction returningAddressLoadingAuditFunction)
        {
            try
            {
                return await returningAddressLoadingAuditFunction();
            }
            catch (NullAddressLoadingAuditException nullAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(nullAddressLoadingAuditException);
            }
            catch (InvalidAddressLoadingAuditException invalidAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(invalidAddressLoadingAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressLoadingAuditStorageException =
                    new FailedAddressLoadingAuditStorageException(
                        message: "Failed addressLoadingAudit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressLoadingAuditStorageException);
            }
        }

        private AddressLoadingAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }

        private AddressLoadingAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressLoadingAuditDependencyException = 
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(addressLoadingAuditDependencyException);

            return addressLoadingAuditDependencyException;
        }
    }
}