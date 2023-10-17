using System.Threading.Tasks;
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
    }
}