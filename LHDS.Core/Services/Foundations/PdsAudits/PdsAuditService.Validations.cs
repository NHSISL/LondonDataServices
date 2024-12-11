// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Foundations.PdsAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.PdsAudits
{
    public partial class PdsAuditService
    {
        private void ValidatePdsAuditOnAdd(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);

            Validate(
                (Rule: IsInvalid(pdsAudit.Id), Parameter: nameof(PdsAudit.Id)),
                (Rule: IsInvalid(pdsAudit.FileName), Parameter: nameof(PdsAudit.FileName)),
                (Rule: IsInvalid(pdsAudit.Message), Parameter: nameof(PdsAudit.Message)),
                (Rule: IsInvalid(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)),
                (Rule: IsInvalid(pdsAudit.CreatedBy), Parameter: nameof(PdsAudit.CreatedBy)),
                (Rule: IsInvalid(pdsAudit.UpdatedDate), Parameter: nameof(PdsAudit.UpdatedDate)),
                (Rule: IsInvalid(pdsAudit.UpdatedBy), Parameter: nameof(PdsAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: pdsAudit.UpdatedDate,
                    secondDate: pdsAudit.CreatedDate,
                    secondDateName: nameof(PdsAudit.CreatedDate)),
                Parameter: nameof(PdsAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: pdsAudit.UpdatedBy,
                    second: pdsAudit.CreatedBy,
                    secondName: nameof(PdsAudit.CreatedBy)),
                Parameter: nameof(PdsAudit.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    pdsAudit.CreatedBy, 255), Parameter: nameof(pdsAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    pdsAudit.UpdatedBy, 255), Parameter: nameof(pdsAudit.UpdatedBy)),

                (Rule: IsNotRecent(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)));
        }

        private void ValidatePdsAuditOnModify(PdsAudit pdsAudit)
        {
            ValidatePdsAuditIsNotNull(pdsAudit);

            Validate(
                (Rule: IsInvalid(pdsAudit.Id), Parameter: nameof(PdsAudit.Id)),
                (Rule: IsInvalid(pdsAudit.FileName), Parameter: nameof(PdsAudit.FileName)),
                (Rule: IsInvalid(pdsAudit.Message), Parameter: nameof(PdsAudit.Message)),
                (Rule: IsInvalid(pdsAudit.CreatedDate), Parameter: nameof(PdsAudit.CreatedDate)),
                (Rule: IsInvalid(pdsAudit.CreatedBy), Parameter: nameof(PdsAudit.CreatedBy)),
                (Rule: IsInvalid(pdsAudit.UpdatedDate), Parameter: nameof(PdsAudit.UpdatedDate)),
                (Rule: IsInvalid(pdsAudit.UpdatedBy), Parameter: nameof(PdsAudit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: pdsAudit.UpdatedDate,
                    secondDate: pdsAudit.CreatedDate,
                    secondDateName: nameof(PdsAudit.CreatedDate)),
                Parameter: nameof(PdsAudit.UpdatedDate)),

                (Rule: IsEqualOrSmallerThan(
                    pdsAudit.CreatedBy, 255), Parameter: nameof(pdsAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    pdsAudit.UpdatedBy, 255), Parameter: nameof(pdsAudit.UpdatedBy)),

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
                throw new NullPdsAuditException(message: "PdsAudit is null.");
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
                    first: inputPdsAudit.CreatedBy,
                    second: storagePdsAudit.CreatedBy,
                    secondName: nameof(PdsAudit.CreatedBy)),
                Parameter: nameof(PdsAudit.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputPdsAudit.UpdatedDate,
                    secondDate: storagePdsAudit.UpdatedDate,
                    secondDateName: nameof(PdsAudit.UpdatedDate)),
                Parameter: nameof(PdsAudit.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsEqualOrSmallerThan(string text, int maxLength) => new
        {
            Condition = (text ?? string.Empty).Length > maxLength,
            Message = "Text is exceeding max length"
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
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
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
            var invalidPdsAuditException = new InvalidPdsAuditException(
                message: "Invalid pdsAudit. Please correct the errors and try again.");

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