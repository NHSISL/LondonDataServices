using System;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditService
    {
        private void ValidateAddressLoadingAuditOnAdd(AddressLoadingAudit addressLoadingAudit)
        {
            ValidateAddressLoadingAuditIsNotNull(addressLoadingAudit);

            Validate(
                (Rule: IsInvalid(addressLoadingAudit.Id), Parameter: nameof(AddressLoadingAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(addressLoadingAudit.CreatedDate), Parameter: nameof(AddressLoadingAudit.CreatedDate)),
                (Rule: IsInvalid(addressLoadingAudit.CreatedBy), Parameter: nameof(AddressLoadingAudit.CreatedBy)),
                (Rule: IsInvalid(addressLoadingAudit.UpdatedDate), Parameter: nameof(AddressLoadingAudit.UpdatedDate)),
                (Rule: IsInvalid(addressLoadingAudit.UpdatedBy), Parameter: nameof(AddressLoadingAudit.UpdatedBy)));
        }

        private static void ValidateAddressLoadingAuditIsNotNull(AddressLoadingAudit addressLoadingAudit)
        {
            if (addressLoadingAudit is null)
            {
                throw new NullAddressLoadingAuditException(message: "AddressLoadingAudit is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

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