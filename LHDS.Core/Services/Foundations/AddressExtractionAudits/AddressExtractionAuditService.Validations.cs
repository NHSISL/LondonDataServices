using System;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditService
    {
        private void ValidateAddressExtractionAuditOnAdd(AddressExtractionAudit addressExtractionAudit)
        {
            ValidateAddressExtractionAuditIsNotNull(addressExtractionAudit);

            Validate(
                (Rule: IsInvalid(addressExtractionAudit.Id), Parameter: nameof(AddressExtractionAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(addressExtractionAudit.CreatedDate), Parameter: nameof(AddressExtractionAudit.CreatedDate)),
                (Rule: IsInvalid(addressExtractionAudit.CreatedBy), Parameter: nameof(AddressExtractionAudit.CreatedBy)),
                (Rule: IsInvalid(addressExtractionAudit.UpdatedDate), Parameter: nameof(AddressExtractionAudit.UpdatedDate)),
                (Rule: IsInvalid(addressExtractionAudit.UpdatedBy), Parameter: nameof(AddressExtractionAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: addressExtractionAudit.UpdatedDate,
                    secondDate: addressExtractionAudit.CreatedDate,
                    secondDateName: nameof(AddressExtractionAudit.CreatedDate)),
                Parameter: nameof(AddressExtractionAudit.UpdatedDate)));
        }

        private static void ValidateAddressExtractionAuditIsNotNull(AddressExtractionAudit addressExtractionAudit)
        {
            if (addressExtractionAudit is null)
            {
                throw new NullAddressExtractionAuditException(message: "AddressExtractionAudit is null.");
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAddressExtractionAuditException = 
                new InvalidAddressExtractionAuditException(
                    message: "Invalid addressExtractionAudit. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAddressExtractionAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAddressExtractionAuditException.ThrowIfContainsErrors();
        }
    }
}