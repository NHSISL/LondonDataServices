// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Landings.Client.Models.Foundations.IngestionTracking.Exceptions;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private void ValidateIngestionTrackingOnAdd(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),
                (Rule: IsInvalid(ingestionTracking.FileName), Parameter: nameof(IngestionTracking.FileName)),
                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsNotRecent(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)));
        }

        private void ValidateIngestionTrackingOnModify(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),
                (Rule: IsInvalid(ingestionTracking.FileName), Parameter: nameof(IngestionTracking.FileName)),
                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)));
        }

        private static void ValidateAgainstStorageIngestionTrackingOnModify(IngestionTracking inputIngestionTracking, IngestionTracking storageIngestionTracking)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputIngestionTracking.CreatedDate,
                    secondDate: storageIngestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.CreatedDate)));
        }


        public void ValidateIngestionTrackingId(string ingestionTrackingsId) =>
            Validate((Rule: IsInvalid(ingestionTrackingsId), Parameter: nameof(IngestionTracking.Id)));

        public void ValidateFileName(string FileName) =>
            Validate((Rule: IsInvalid(FileName), Parameter: nameof(IngestionTracking.FileName)));

        private static void ValidateStorageIngestionTracking(IngestionTracking maybeIngestionTracking, string ingestionTrackingId)
        {
            if (maybeIngestionTracking is null)
            {
                throw new NotFoundIngestionTrackingException(ingestionTrackingId);
            }
        }

        private static void ValidateStorageIngestionTrackingForFileName(IngestionTracking maybeIngestionTracking, string fileName)
        {
            if (maybeIngestionTracking is null)
            {
                throw new NotFoundIngestionTrackingForFileNameException(fileName);
            }
        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking ingestionTrackings)
        {
            if (ingestionTrackings is null)
            {
                throw new NullIngestionTrackingException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(Guid? id) => new
        {
            Condition = id.HasValue && id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsEqualOrSmallerThan(string text, int maxLength) => new
        {
            Condition = (text ?? string.Empty).Length > maxLength,
            Message = "Text is exceeding max length"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
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
                 string firstUser,
                 string secondUser,
                 string secondUserName) => new
                 {
                     Condition = firstUser != secondUser,
                     Message = $"User is not the same as {secondUserName}"
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
            var invalidIngestionTrackingException = new InvalidIngestionTrackingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidIngestionTrackingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidIngestionTrackingException.ThrowIfContainsErrors();
        }
    }
}
