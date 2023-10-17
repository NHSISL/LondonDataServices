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
                Parameter: nameof(AddressExtractionAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: addressExtractionAudit.UpdatedBy,
                    second: addressExtractionAudit.CreatedBy,
                    secondName: nameof(AddressExtractionAudit.CreatedBy)),
                Parameter: nameof(AddressExtractionAudit.UpdatedBy)),

                (Rule: IsNotRecent(addressExtractionAudit.CreatedDate), Parameter: nameof(AddressExtractionAudit.CreatedDate)));
        }

        private void ValidateAddressExtractionAuditOnModify(AddressExtractionAudit addressExtractionAudit)
        {
            ValidateAddressExtractionAuditIsNotNull(addressExtractionAudit);

            Validate(
                (Rule: IsInvalid(addressExtractionAudit.Id), Parameter: nameof(AddressExtractionAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(addressExtractionAudit.CreatedDate), Parameter: nameof(AddressExtractionAudit.CreatedDate)),
                (Rule: IsInvalid(addressExtractionAudit.CreatedBy), Parameter: nameof(AddressExtractionAudit.CreatedBy)),
                (Rule: IsInvalid(addressExtractionAudit.UpdatedDate), Parameter: nameof(AddressExtractionAudit.UpdatedDate)),
                (Rule: IsInvalid(addressExtractionAudit.UpdatedBy), Parameter: nameof(AddressExtractionAudit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: addressExtractionAudit.UpdatedDate,
                    secondDate: addressExtractionAudit.CreatedDate,
                    secondDateName: nameof(AddressExtractionAudit.CreatedDate)),
                Parameter: nameof(AddressExtractionAudit.UpdatedDate)),

                (Rule: IsNotRecent(addressExtractionAudit.UpdatedDate), Parameter: nameof(addressExtractionAudit.UpdatedDate)));
        }

        public void ValidateAddressExtractionAuditId(Guid addressExtractionAuditId) =>
            Validate((Rule: IsInvalid(addressExtractionAuditId), Parameter: nameof(AddressExtractionAudit.Id)));

        private static void ValidateStorageAddressExtractionAudit(AddressExtractionAudit maybeAddressExtractionAudit, Guid addressExtractionAuditId)
        {
            if (maybeAddressExtractionAudit is null)
            {
                throw new NotFoundAddressExtractionAuditException(addressExtractionAuditId);
            }
        }

        private static void ValidateAddressExtractionAuditIsNotNull(AddressExtractionAudit addressExtractionAudit)
        {
            if (addressExtractionAudit is null)
            {
                throw new NullAddressExtractionAuditException(message: "AddressExtractionAudit is null.");
            }
        }

        private static void ValidateAgainstStorageAddressExtractionAuditOnModify(AddressExtractionAudit inputAddressExtractionAudit, AddressExtractionAudit storageAddressExtractionAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAddressExtractionAudit.CreatedDate,
                    secondDate: storageAddressExtractionAudit.CreatedDate,
                    secondDateName: nameof(AddressExtractionAudit.CreatedDate)),
                Parameter: nameof(AddressExtractionAudit.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputAddressExtractionAudit.CreatedBy,
                    second: storageAddressExtractionAudit.CreatedBy,
                    secondName: nameof(AddressExtractionAudit.CreatedBy)),
                Parameter: nameof(AddressExtractionAudit.CreatedBy)));
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

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