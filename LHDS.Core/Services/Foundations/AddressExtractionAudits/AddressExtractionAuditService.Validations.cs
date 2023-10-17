using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditService
    {
        private void ValidateAddressExtractionAuditOnAdd(AddressExtractionAudit addressExtractionAudit)
        {
            ValidateAddressExtractionAuditIsNotNull(addressExtractionAudit);
        }

        private static void ValidateAddressExtractionAuditIsNotNull(AddressExtractionAudit addressExtractionAudit)
        {
            if (addressExtractionAudit is null)
            {
                throw new NullAddressExtractionAuditException(message: "AddressExtractionAudit is null.");
            }
        }
    }
}