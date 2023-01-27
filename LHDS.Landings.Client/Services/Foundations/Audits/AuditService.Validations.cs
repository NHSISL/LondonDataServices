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
                (Rule: IsInvalid(audit.UpdatedByUserId), Parameter: nameof(Audit.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: audit.UpdatedDate,
                    secondDate: audit.CreatedDate,
                    secondDateName: nameof(Audit.CreatedDate)),
                Parameter: nameof(Audit.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: audit.UpdatedByUserId,
                    secondId: audit.CreatedByUserId,
                    secondIdName: nameof(Audit.CreatedByUserId)),
                Parameter: nameof(Audit.UpdatedByUserId)),

                (Rule: IsNotRecent(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)));
        }

        private void ValidateAuditOnModify(Audit audit)
        {
            ValidateAuditIsNotNull(audit);

            Validate(
                (Rule: IsInvalid(audit.Id), Parameter: nameof(Audit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)),
                (Rule: IsInvalid(audit.CreatedByUserId), Parameter: nameof(Audit.CreatedByUserId)),
                (Rule: IsInvalid(audit.UpdatedDate), Parameter: nameof(Audit.UpdatedDate)),
                (Rule: IsInvalid(audit.UpdatedByUserId), Parameter: nameof(Audit.UpdatedByUserId)),

                (Rule: IsSame(
                    firstDate: audit.UpdatedDate,
                    secondDate: audit.CreatedDate,
                    secondDateName: nameof(Audit.CreatedDate)),
                Parameter: nameof(Audit.UpdatedDate)));
        }

        public void ValidateAuditId(Guid auditId) =>
            Validate((Rule: IsInvalid(auditId), Parameter: nameof(Audit.Id)));

        private static void ValidateStorageAudit(Audit maybeAudit, Guid auditId)
        {
            if (maybeAudit is null)
            {
                throw new NotFoundAuditException(auditId);
            }
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