using System;
using LHDS.Core.Models.PdsAudits;
using LHDS.Core.Models.PdsAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private void ValidatePdsAuditOnAdd(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);

            Validate(
                (Rule: IsInvalid(pdsAudit.Id), Parameter: nameof(PdsAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)),
                (Rule: IsInvalid(pdsAudit.CreatedByUserId), Parameter: nameof(PdsAudit.CreatedByUserId)),
                (Rule: IsInvalid(pdsAudit.UpdatedDate), Parameter: nameof(PdsAudit.UpdatedDate)),
                (Rule: IsInvalid(pdsAudit.UpdatedByUserId), Parameter: nameof(PdsAudit.UpdatedByUserId)));
        }

        private static void ValidatePdsAuditIsNotNull(PdsAudit pdsAudit)
        {
            if (pdsAudit is null)
            {
                throw new NullPdsAuditException();
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
            var invalidPdsAuditException = new InvalidPdsAuditException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPdsAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPdsAuditException.ThrowIfContainsErrors();
        }
    }
}