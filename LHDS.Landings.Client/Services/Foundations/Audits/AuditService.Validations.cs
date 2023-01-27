using System;
using LHDS.Landings.Client.Models.Audits;
using LHDS.Landings.Client.Models.Audits.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.Audits
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
                (Rule: IsInvalid(audit.CreatedByUserId), Parameter: nameof(Audit.CreatedByUserId)),
                (Rule: IsInvalid(audit.UpdatedDate), Parameter: nameof(Audit.UpdatedDate)),
                (Rule: IsInvalid(audit.UpdatedByUserId), Parameter: nameof(Audit.UpdatedByUserId)));
        }

        private static void ValidateAuditIsNotNull(Audit audit)
        {
            if (audit is null)
            {
                throw new NullAuditException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAuditException = new InvalidAuditException();

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