// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditService
    {
        private void ValidateIngestionTrackingAuditOnAdd(IngestionTrackingAudit ingestionTrackingAudit)
        {
            ValidateIngestionTrackingAuditIsNotNull(ingestionTrackingAudit);

            Validate(
                (Rule: IsInvalid(ingestionTrackingAudit.Id), Parameter: nameof(IngestionTrackingAudit.Id)),

                (Rule: IsInvalid(ingestionTrackingAudit.IngestionTrackingId),
                    Parameter: nameof(IngestionTrackingAudit.IngestionTrackingId)),

                (Rule: IsInvalid(ingestionTrackingAudit.Message),
                    Parameter: nameof(IngestionTrackingAudit.Message)),

                (Rule: IsInvalid(ingestionTrackingAudit.CreatedDate),
                    Parameter: nameof(IngestionTrackingAudit.CreatedDate)),

                (Rule: IsInvalid(ingestionTrackingAudit.CreatedBy),
                    Parameter: nameof(IngestionTrackingAudit.CreatedBy)),

                (Rule: IsInvalid(ingestionTrackingAudit.UpdatedDate),
                    Parameter: nameof(IngestionTrackingAudit.UpdatedDate)),

                (Rule: IsInvalid(ingestionTrackingAudit.UpdatedBy),
                    Parameter: nameof(IngestionTrackingAudit.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: ingestionTrackingAudit.UpdatedDate,
                    secondDate: ingestionTrackingAudit.CreatedDate,
                    secondDateName: nameof(IngestionTrackingAudit.CreatedDate)),
                Parameter: nameof(IngestionTrackingAudit.UpdatedDate)),

                (Rule: IsNotSame(
                    first: ingestionTrackingAudit.UpdatedBy,
                    second: ingestionTrackingAudit.CreatedBy,
                    secondName: nameof(IngestionTrackingAudit.CreatedBy)),
                Parameter: nameof(IngestionTrackingAudit.UpdatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    ingestionTrackingAudit.CreatedBy, 255), Parameter: nameof(ingestionTrackingAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    ingestionTrackingAudit.UpdatedBy, 255), Parameter: nameof(ingestionTrackingAudit.UpdatedBy)),

                (Rule: IsNotRecent(ingestionTrackingAudit.CreatedDate),
                    Parameter: nameof(IngestionTrackingAudit.CreatedDate)));
        }

        private void ValidateIngestionTrackingAuditOnModify(IngestionTrackingAudit ingestionTrackingAudit)
        {
            ValidateIngestionTrackingAuditIsNotNull(ingestionTrackingAudit);

            Validate(
                (Rule: IsInvalid(ingestionTrackingAudit.Id), Parameter: nameof(IngestionTrackingAudit.Id)),

                (Rule: IsInvalid(ingestionTrackingAudit.IngestionTrackingId),
                    Parameter: nameof(IngestionTrackingAudit.IngestionTrackingId)),

                (Rule: IsInvalid(ingestionTrackingAudit.Message),
                    Parameter: nameof(IngestionTrackingAudit.Message)),

                (Rule: IsInvalid(ingestionTrackingAudit.CreatedDate),
                    Parameter: nameof(IngestionTrackingAudit.CreatedDate)),

                (Rule: IsInvalid(ingestionTrackingAudit.CreatedBy),
                    Parameter: nameof(IngestionTrackingAudit.CreatedBy)),

                (Rule: IsInvalid(ingestionTrackingAudit.UpdatedDate),
                    Parameter: nameof(IngestionTrackingAudit.UpdatedDate)),

                (Rule: IsInvalid(ingestionTrackingAudit.UpdatedBy),
                    Parameter: nameof(IngestionTrackingAudit.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: ingestionTrackingAudit.UpdatedDate,
                    secondDate: ingestionTrackingAudit.CreatedDate,
                    secondDateName: nameof(IngestionTrackingAudit.CreatedDate)),
                Parameter: nameof(IngestionTrackingAudit.UpdatedDate)),

                (Rule: IsEqualOrSmallerThan(
                    ingestionTrackingAudit.CreatedBy, 255), Parameter: nameof(ingestionTrackingAudit.CreatedBy)),

                (Rule: IsEqualOrSmallerThan(
                    ingestionTrackingAudit.UpdatedBy, 255), Parameter: nameof(ingestionTrackingAudit.UpdatedBy)),

                (Rule: IsNotRecent(ingestionTrackingAudit.UpdatedDate),
                    Parameter: nameof(ingestionTrackingAudit.UpdatedDate)));
        }

        public void ValidateIngestionTrackingAuditId(Guid ingestionTrackingAuditId) =>
            Validate((Rule: IsInvalid(ingestionTrackingAuditId), Parameter: nameof(IngestionTrackingAudit.Id)));

        private static void ValidateStorageIngestionTrackingAudit(
            IngestionTrackingAudit maybeIngestionTrackingAudit,
            Guid ingestionTrackingAuditId)
        {
            if (maybeIngestionTrackingAudit is null)
            {
                throw new NotFoundIngestionTrackingAuditException(
                    message: $"Couldn't find IngestionTrackingAudit with Id: " +
                        $"{ingestionTrackingAuditId}.");
            }
        }

        private static void ValidateIngestionTrackingAuditIsNotNull(IngestionTrackingAudit ingestionTrackingAudit)
        {
            if (ingestionTrackingAudit is null)
            {
                throw new NullIngestionTrackingAuditException(message: "IngestionTrackingAudit is null.");
            }
        }

        private static void ValidateIngestionTrackingAuditIsNull(IngestionTrackingAudit ingestionTrackingAudit)
        {
            if (ingestionTrackingAudit is not null)
            {
                throw new InvalidIngestionTrackingAuditException(message: "IngestionTrackingAudit is not null.");
            }
        }

        private static void ValidateAgainstStorageIngestionTrackingAuditOnModify(
            IngestionTrackingAudit inputIngestionTrackingAudit,
            IngestionTrackingAudit storageIngestionTrackingAudit)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputIngestionTrackingAudit.CreatedDate,
                    secondDate: storageIngestionTrackingAudit.CreatedDate,
                    secondDateName: nameof(IngestionTrackingAudit.CreatedDate)),
                Parameter: nameof(IngestionTrackingAudit.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputIngestionTrackingAudit.CreatedBy,
                    second: storageIngestionTrackingAudit.CreatedBy,
                    secondName: nameof(IngestionTrackingAudit.CreatedBy)),
                Parameter: nameof(IngestionTrackingAudit.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputIngestionTrackingAudit.UpdatedDate,
                    secondDate: storageIngestionTrackingAudit.UpdatedDate,
                    secondDateName: nameof(IngestionTrackingAudit.UpdatedDate)),
                Parameter: nameof(IngestionTrackingAudit.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
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
            var invalidIngestionTrackingAuditException = new InvalidIngestionTrackingAuditException(
                message: "Invalid IngestionTrackingAudit. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIngestionTrackingAuditException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidIngestionTrackingAuditException.ThrowIfContainsErrors();
        }
    }
}