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
                (Rule: IsInvalid(addressLoadingAudit.UpdatedBy), Parameter: nameof(AddressLoadingAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: addressLoadingAudit.UpdatedDate,
                    secondDate: addressLoadingAudit.CreatedDate,
                    secondDateName: nameof(AddressLoadingAudit.CreatedDate)),
                Parameter: nameof(AddressLoadingAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: addressLoadingAudit.UpdatedBy,
                    second: addressLoadingAudit.CreatedBy,
                    secondName: nameof(AddressLoadingAudit.CreatedBy)),
                Parameter: nameof(AddressLoadingAudit.UpdatedBy)),

                (Rule: IsNotRecent(addressLoadingAudit.CreatedDate), Parameter: nameof(AddressLoadingAudit.CreatedDate)));
        }

        private void ValidateAddressLoadingAuditOnModify(AddressLoadingAudit addressLoadingAudit)
        {
            ValidateAddressLoadingAuditIsNotNull(addressLoadingAudit);

            Validate(
                (Rule: IsInvalid(addressLoadingAudit.Id), Parameter: nameof(AddressLoadingAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(addressLoadingAudit.CreatedDate), Parameter: nameof(AddressLoadingAudit.CreatedDate)),
                (Rule: IsInvalid(addressLoadingAudit.CreatedBy), Parameter: nameof(AddressLoadingAudit.CreatedBy)),
                (Rule: IsInvalid(addressLoadingAudit.UpdatedDate), Parameter: nameof(AddressLoadingAudit.UpdatedDate)),
                (Rule: IsInvalid(addressLoadingAudit.UpdatedBy), Parameter: nameof(AddressLoadingAudit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: addressLoadingAudit.UpdatedDate,
                    secondDate: addressLoadingAudit.CreatedDate,
                    secondDateName: nameof(AddressLoadingAudit.CreatedDate)),
                Parameter: nameof(AddressLoadingAudit.UpdatedDate)),

                (Rule: IsNotRecent(addressLoadingAudit.UpdatedDate), Parameter: nameof(addressLoadingAudit.UpdatedDate)));
        }

        public void ValidateAddressLoadingAuditId(Guid addressLoadingAuditId) =>
            Validate((Rule: IsInvalid(addressLoadingAuditId), Parameter: nameof(AddressLoadingAudit.Id)));

        private static void ValidateStorageAddressLoadingAudit(AddressLoadingAudit maybeAddressLoadingAudit, Guid addressLoadingAuditId)
        {
            if (maybeAddressLoadingAudit is null)
            {
                throw new NotFoundAddressLoadingAuditException(addressLoadingAuditId);
            }
        }

        private static void ValidateAddressLoadingAuditIsNotNull(AddressLoadingAudit addressLoadingAudit)
        {
            if (addressLoadingAudit is null)
            {
                throw new NullAddressLoadingAuditException(message: "AddressLoadingAudit is null.");
            }
        }

        private static void ValidateAgainstStorageAddressLoadingAuditOnModify(AddressLoadingAudit inputAddressLoadingAudit, AddressLoadingAudit storageAddressLoadingAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAddressLoadingAudit.CreatedDate,
                    secondDate: storageAddressLoadingAudit.CreatedDate,
                    secondDateName: nameof(AddressLoadingAudit.CreatedDate)),
                Parameter: nameof(AddressLoadingAudit.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputAddressLoadingAudit.CreatedBy,
                    second: storageAddressLoadingAudit.CreatedBy,
                    secondName: nameof(AddressLoadingAudit.CreatedBy)),
                Parameter: nameof(AddressLoadingAudit.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputAddressLoadingAudit.UpdatedDate,
                    secondDate: storageAddressLoadingAudit.UpdatedDate,
                    secondDateName: nameof(AddressLoadingAudit.UpdatedDate)),
                Parameter: nameof(AddressLoadingAudit.UpdatedDate)));
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