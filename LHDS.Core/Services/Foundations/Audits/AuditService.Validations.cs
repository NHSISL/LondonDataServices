// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
                (Rule: IsInvalid(audit.IngestionTrackingId), Parameter: nameof(Audit.IngestionTrackingId)),
                (Rule: IsInvalid(audit.Message), Parameter: nameof(Audit.Message)),
                (Rule: IsInvalid(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)),
                (Rule: IsInvalid(audit.CreatedBy), Parameter: nameof(Audit.CreatedBy)),
                (Rule: IsInvalid(audit.UpdatedDate), Parameter: nameof(Audit.UpdatedDate)),
                (Rule: IsInvalid(audit.UpdatedBy), Parameter: nameof(Audit.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: audit.UpdatedDate,
                    secondDate: audit.CreatedDate,
                    secondDateName: nameof(Audit.CreatedDate)),
                Parameter: nameof(Audit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: audit.UpdatedBy,
                    second: audit.CreatedBy,
                    secondName: nameof(Audit.CreatedBy)),
                Parameter: nameof(Audit.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    audit.CreatedBy, 255), Parameter: nameof(audit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    audit.UpdatedBy, 255), Parameter: nameof(audit.UpdatedBy)),

                (Rule: IsNotRecent(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)));
        }

        private void ValidateAuditOnModify(Audit audit)
        {
            ValidateAuditIsNotNull(audit);

            Validate(
                (Rule: IsInvalid(audit.Id), Parameter: nameof(Audit.Id)),
                (Rule: IsInvalid(audit.IngestionTrackingId), Parameter: nameof(Audit.IngestionTrackingId)),
                (Rule: IsInvalid(audit.Message), Parameter: nameof(Audit.Message)),
                (Rule: IsInvalid(audit.CreatedDate), Parameter: nameof(Audit.CreatedDate)),
                (Rule: IsInvalid(audit.CreatedBy), Parameter: nameof(Audit.CreatedBy)),
                (Rule: IsInvalid(audit.UpdatedDate), Parameter: nameof(Audit.UpdatedDate)),
                (Rule: IsInvalid(audit.UpdatedBy), Parameter: nameof(Audit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: audit.UpdatedDate,
                    secondDate: audit.CreatedDate,
                    secondDateName: nameof(Audit.CreatedDate)),
                Parameter: nameof(Audit.UpdatedDate)),

                (Rule: IsEqualOrSmallerThan(
                    audit.CreatedBy, 255), Parameter: nameof(audit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    audit.UpdatedBy, 255), Parameter: nameof(audit.UpdatedBy)),

                (Rule: IsNotRecent(audit.UpdatedDate), Parameter: nameof(audit.UpdatedDate)));
        }

        public void ValidateAuditId(Guid auditId) =>
            Validate((Rule: IsInvalid(auditId), Parameter: nameof(Audit.Id)));

        private static void ValidateStorageAudit(Audit maybeAudit, Guid auditId)
        {
            if (maybeAudit is null)
            {
                throw new NotFoundAuditException(message: $"Couldn't find audit with auditId: {auditId}.");
            }
        }

        private static void ValidateAuditIsNotNull(Audit audit)
        {
            if (audit is null)
            {
                throw new NullAuditException(message: "Audit is null.");
            }
        }

        private static void ValidateAuditIsNull(Audit audit)
        {
            if (audit is not null)
            {
                throw new InvalidAuditException(message: "Audit is not null.");
            }
        }

        private static void ValidateAgainstStorageAuditOnModify(Audit inputAudit, Audit storageAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAudit.CreatedDate,
                    secondDate: storageAudit.CreatedDate,
                    secondDateName: nameof(Audit.CreatedDate)),
                Parameter: nameof(Audit.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputAudit.CreatedBy,
                    second: storageAudit.CreatedBy,
                    secondName: nameof(Audit.CreatedBy)),
                Parameter: nameof(Audit.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputAudit.UpdatedDate,
                    secondDate: storageAudit.UpdatedDate,
                    secondDateName: nameof(Audit.UpdatedDate)),
                Parameter: nameof(Audit.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
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
            var invalidAuditException = new InvalidAuditException(
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