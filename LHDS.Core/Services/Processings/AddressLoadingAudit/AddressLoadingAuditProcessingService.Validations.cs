using System;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;

namespace LHDS.Core.Services.Processings.AddressLoadingAudits
{
    public partial class AddressLoadingAuditProcessingService
    {
        private void ValidateAddressLoadingAuditOnAdd(AddressLoadingAudit addressLoadingAudit)
        {
            ValidateAddressLoadingAuditIsNotNull(addressLoadingAudit);
        }

        private static void ValidateAddressLoadingAuditIsNotNull(AddressLoadingAudit addressLoadingAudit)
        {
            if (addressLoadingAudit is null)
            {
                throw new NullAddressLoadingAuditException(message: "Address loading audit is null.");
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAddressLoadingAuditException = 
                new InvalidAddressLoadingAuditException(
                    message: "Invalid addressLoadingAudit. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAddressLoadingAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAddressLoadingAuditException.ThrowIfContainsErrors();
        }
    }
}