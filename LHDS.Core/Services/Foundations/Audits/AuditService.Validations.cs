using System;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Audits.Exceptions;

namespace LHDS.Core.Services.Foundations.Audits
{
    public partial class AuditService
    {
        private void ValidateAuditOnAdd(Audit audit)
        {
            ValidateAuditIsNotNull(audit);

            Validate(
                (Rule: IsInvalid(audit.Id), Parameter: nameof(Audit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)),
                (Rule: IsInvalid(audit.CreatedBy), Parameter: nameof(Audit.CreatedBy)),
                (Rule: IsInvalid(audit.UpdatedDate), Parameter: nameof(Audit.UpdatedDate)),
                (Rule: IsInvalid(audit.UpdatedBy), Parameter: nameof(Audit.UpdatedBy)));
        }

        private static void ValidateAuditIsNotNull(Audit audit)
        {
            if (audit is null)
            {
                throw new NullAuditException(message: "Audit is null.");
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
            var invalidAuditException = 
                new InvalidAuditException(
                    message: "Invalid audit. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAuditException.ThrowIfContainsErrors();
        }
    }
}