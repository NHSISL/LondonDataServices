using System;
using LHDS.Core.Models.IngestionTrackings;
using LHDS.Core.Models.IngestionTrackings.Exceptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private void ValidateIngestionTrackingOnAdd(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(ingestionTracking.CreatedByUserId), Parameter: nameof(IngestionTracking.CreatedByUserId)),
                (Rule: IsInvalid(ingestionTracking.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(ingestionTracking.UpdatedByUserId), Parameter: nameof(IngestionTracking.UpdatedByUserId)),

                (Rule: IsNotSame(
                    firstDate: ingestionTracking.UpdatedDate,
                    secondDate: ingestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: ingestionTracking.UpdatedByUserId,
                    secondId: ingestionTracking.CreatedByUserId,
                    secondIdName: nameof(IngestionTracking.CreatedByUserId)),
                Parameter: nameof(IngestionTracking.UpdatedByUserId)),

                (Rule: IsNotRecent(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)));
        }

        private void ValidateIngestionTrackingOnModify(IngestionTracking ingestionTracking)
        {
            ValidateIngestionTrackingIsNotNull(ingestionTracking);
        }

        public void ValidateIngestionTrackingId(Guid ingestionTrackingId) =>
            Validate((Rule: IsInvalid(ingestionTrackingId), Parameter: nameof(IngestionTracking.Id)));

        private static void ValidateStorageIngestionTracking(IngestionTracking maybeIngestionTracking, Guid ingestionTrackingId)
        {
            if (maybeIngestionTracking is null)
            {
                throw new NotFoundIngestionTrackingException(ingestionTrackingId);
            }
        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking ingestionTracking)
        {
            if (ingestionTracking is null)
            {
                throw new NullIngestionTrackingException();
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