using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditService
    {
        private void ValidateAddressLoadingAuditOnAdd(AddressLoadingAudit addressLoadingAudit)
        {
            ValidateAddressLoadingAuditIsNotNull(addressLoadingAudit);
        }

        private static void ValidateAddressLoadingAuditIsNotNull(AddressLoadingAudit addressLoadingAudit)
        {
            if (addressLoadingAudit is null)
            {
                throw new NullAddressLoadingAuditException(message: "AddressLoadingAudit is null.");
            }
        }
    }
}