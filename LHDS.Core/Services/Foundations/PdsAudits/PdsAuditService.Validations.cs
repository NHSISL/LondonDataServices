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
                (Rule: IsInvalid(pdsAudit.UpdatedByUserId), Parameter: nameof(PdsAudit.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: pdsAudit.UpdatedDate,
                    secondDate: pdsAudit.CreatedDate,
                    secondDateName: nameof(PdsAudit.CreatedDate)),
                Parameter: nameof(PdsAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: pdsAudit.UpdatedByUserId,
                    secondId: pdsAudit.CreatedByUserId,
                    secondIdName: nameof(PdsAudit.CreatedByUserId)),
                Parameter: nameof(PdsAudit.UpdatedByUserId)),

                (Rule: IsNotRecent(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)));
        }

        private void ValidatePdsAuditOnModify(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);

            Validate(
                (Rule: IsInvalid(pdsAudit.Id), Parameter: nameof(PdsAudit.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)),
                (Rule: IsInvalid(pdsAudit.CreatedByUserId), Parameter: nameof(PdsAudit.CreatedByUserId)),
                (Rule: IsInvalid(pdsAudit.UpdatedDate), Parameter: nameof(PdsAudit.UpdatedDate)),
                (Rule: IsInvalid(pdsAudit.UpdatedByUserId), Parameter: nameof(PdsAudit.UpdatedByUserId)),

                (Rule: IsSame(
                    firstDate: pdsAudit.UpdatedDate,
                    secondDate: pdsAudit.CreatedDate,
                    secondDateName: nameof(PdsAudit.CreatedDate)),
                Parameter: nameof(PdsAudit.UpdatedDate)),

                (Rule: IsNotRecent(pdsAudit.UpdatedDate), Parameter: nameof(pdsAudit.UpdatedDate)));
        }

        public void ValidatePdsAuditId(Guid pdsAuditId) =>
            Validate((Rule: IsInvalid(pdsAuditId), Parameter: nameof(PdsAudit.Id)));

        private static void ValidateStoragePdsAudit(PdsAudit maybePdsAudit, Guid pdsAuditId)
        {
            if (maybePdsAudit is null)
            {
                throw new NotFoundPdsAuditException(pdsAuditId);
            }
        }

        private static void ValidatePdsAuditIsNotNull(PdsAudit pdsAudit)
        {
            if (pdsAudit is null)
            {
                throw new NullPdsAuditException();
            }
        }

        private static void ValidateAgainstStoragePdsAuditOnModify(PdsAudit inputPdsAudit, PdsAudit storagePdsAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputPdsAudit.CreatedDate,
                    secondDate: storagePdsAudit.CreatedDate,
                    secondDateName: nameof(PdsAudit.CreatedDate)),
                Parameter: nameof(PdsAudit.CreatedDate)),

                (Rule: IsNotSame(
                    firstId: inputPdsAudit.CreatedByUserId,
                    secondId: storagePdsAudit.CreatedByUserId,
                    secondIdName: nameof(PdsAudit.CreatedByUserId)),
                Parameter: nameof(PdsAudit.CreatedByUserId)));
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