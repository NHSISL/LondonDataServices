using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditService
    {
        private delegate ValueTask<AddressExtractionAudit> ReturningAddressExtractionAuditFunction();

        private async ValueTask<AddressExtractionAudit> TryCatch(ReturningAddressExtractionAuditFunction returningAddressExtractionAuditFunction)
        {
            try
            {
                return await returningAddressExtractionAuditFunction();
            }
            catch (NullAddressExtractionAuditException nullAddressExtractionAuditException)
            {
                throw CreateAndLogValidationException(nullAddressExtractionAuditException);
            }
            catch (InvalidAddressExtractionAuditException invalidAddressExtractionAuditException)
            {
                throw CreateAndLogValidationException(invalidAddressExtractionAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressExtractionAuditStorageException =
                    new FailedAddressExtractionAuditStorageException(
                        message: "Failed addressExtractionAudit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressExtractionAuditStorageException);
            }
        }

        private AddressExtractionAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionAuditValidationException);

            return addressExtractionAuditValidationException;
        }

        private AddressExtractionAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressExtractionAuditDependencyException = 
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(addressExtractionAuditDependencyException);

            return addressExtractionAuditDependencyException;
        }
    }
}