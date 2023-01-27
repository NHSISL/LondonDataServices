// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings;
using LHDS.Landings.Client.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.Landings.Client.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private void ValidateIngestionTrackingOnAdd(IngestionTracking ingestionTrackings)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTrackings);

            Validate(
                (Rule: IsInvalid(ingestionTrackings.Id), Parameter: nameof(IngestionTracking.Id)),
                (Rule: IsInvalid(ingestionTrackings.Name), Parameter: nameof(IngestionTracking.Name)),
                (Rule: IsInvalid(ingestionTrackings.EncryptedBlobId), Parameter: nameof(IngestionTracking.EncryptedBlobId)),
                (Rule: IsInvalid(ingestionTrackings.DecryptedBlobId), Parameter: nameof(IngestionTracking.DecryptedBlobId)),
                (Rule: IsInvalid(ingestionTrackings.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(ingestionTrackings.CreatedBy), Parameter: nameof(IngestionTracking.CreatedBy)),
                (Rule: IsInvalid(ingestionTrackings.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(ingestionTrackings.UpdatedBy), Parameter: nameof(IngestionTracking.UpdatedBy)),
                (Rule: IsEqualOrSmallerThan(ingestionTrackings.Name, 255), Parameter: nameof(IngestionTracking.Name)),

                (Rule: IsNotSame(
                    firstDate: ingestionTrackings.UpdatedDate,
                    secondDate: ingestionTrackings.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)),

                (Rule: IsNotSame(
                    firstUser: ingestionTrackings.UpdatedBy,
                    secondUser: ingestionTrackings.CreatedBy,
                    secondUserName: nameof(IngestionTracking.CreatedBy)),
                Parameter: nameof(IngestionTracking.UpdatedBy)),

                (Rule: IsNotRecent(ingestionTrackings.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)));
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
