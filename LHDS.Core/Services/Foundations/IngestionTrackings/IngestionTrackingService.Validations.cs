// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;

namespace LHDS.Core.Services.Foundations.IngestionTrackings
{
    public partial class IngestionTrackingService
    {
        private async ValueTask ValidateIngestionTrackingOnAddAsync(IngestionTracking ingestionTracking)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),
                (Rule: IsInvalid(ingestionTracking.FileName), Parameter: nameof(IngestionTracking.FileName)),
                (Rule: IsInvalid(ingestionTracking.SupplierId), Parameter: nameof(IngestionTracking.SupplierId)),
                (Rule: IsInvalid(ingestionTracking.EncryptedFileName),
                    Parameter: nameof(IngestionTracking.EncryptedFileName)),
                (Rule: IsInvalid(ingestionTracking.DecryptedFileName),
                    Parameter: nameof(IngestionTracking.DecryptedFileName)),
                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(ingestionTracking.CreatedBy), Parameter: nameof(IngestionTracking.CreatedBy)),
                (Rule: IsInvalid(ingestionTracking.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(ingestionTracking.UpdatedBy), Parameter: nameof(IngestionTracking.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: ingestionTracking.CreatedBy),
                Parameter: nameof(IngestionTracking.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: ingestionTracking.UpdatedDate,
                    secondDate: ingestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)),

                (Rule: IsNotSame(
                    first: ingestionTracking.UpdatedBy,
                    second: ingestionTracking.CreatedBy,
                    secondName: nameof(IngestionTracking.CreatedBy)),
                Parameter: nameof(IngestionTracking.UpdatedBy)),

                (Rule: await IsNotRecentAsync(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)));
        }

        private async ValueTask ValidateIngestionTrackingOnModifyAsync(IngestionTracking ingestionTracking)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(ingestionTracking.Id), Parameter: nameof(IngestionTracking.Id)),
                (Rule: IsInvalid(ingestionTracking.FileName), Parameter: nameof(IngestionTracking.FileName)),
                (Rule: IsInvalid(ingestionTracking.SupplierId), Parameter: nameof(IngestionTracking.SupplierId)),
                (Rule: IsInvalid(ingestionTracking.EncryptedFileName),
                    Parameter: nameof(IngestionTracking.EncryptedFileName)),
                (Rule: IsInvalid(ingestionTracking.DecryptedFileName),
                    Parameter: nameof(IngestionTracking.DecryptedFileName)),
                (Rule: IsInvalid(ingestionTracking.CreatedDate), Parameter: nameof(IngestionTracking.CreatedDate)),
                (Rule: IsInvalid(ingestionTracking.CreatedBy), Parameter: nameof(IngestionTracking.CreatedBy)),
                (Rule: IsInvalid(ingestionTracking.UpdatedDate), Parameter: nameof(IngestionTracking.UpdatedDate)),
                (Rule: IsInvalid(ingestionTracking.UpdatedBy), Parameter: nameof(IngestionTracking.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: ingestionTracking.UpdatedBy),
                Parameter: nameof(IngestionTracking.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: ingestionTracking.UpdatedDate,
                    secondDate: ingestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)),

                (Rule: await IsNotRecentAsync(ingestionTracking.UpdatedDate), Parameter: nameof(ingestionTracking.UpdatedDate)));
        }

        public void ValidateIngestionTrackingId(Guid ingestionTrackingId) =>
            Validate((Rule: IsInvalid(ingestionTrackingId), Parameter: nameof(IngestionTracking.Id)));

        public void ValidateIngestionTrackingFileName(string fileName) =>
            Validate((Rule: IsInvalid(fileName), Parameter: nameof(IngestionTracking.FileName)));

        private static void ValidateStorageIngestionTracking(
            IngestionTracking maybeIngestionTracking,
            Guid ingestionTrackingId)
        {
            if (maybeIngestionTracking is null)
            {
                throw new NotFoundIngestionTrackingException(ingestionTrackingId);
            }
        }

        private static void ValidateStorageIngestionTracking(
            IngestionTracking maybeIngestionTracking,
            string fileName)
        {
            if (maybeIngestionTracking is null)
            {
                throw new NotFoundIngestionTrackingException(fileName);
            }
        }

        private static void ValidateIngestionTrackingIsNotNull(IngestionTracking ingestionTracking)
        {
            if (ingestionTracking is null)
            {
                throw new NullIngestionTrackingException(message: "Ingestion tracking is null.");
            }
        }

        private static void ValidateAgainstStorageIngestionTrackingOnModify(
            IngestionTracking inputIngestionTracking,
            IngestionTracking storageIngestionTracking)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputIngestionTracking.CreatedDate,
                    secondDate: storageIngestionTracking.CreatedDate,
                    secondDateName: nameof(IngestionTracking.CreatedDate)),
                Parameter: nameof(IngestionTracking.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputIngestionTracking.CreatedBy,
                    second: storageIngestionTracking.CreatedBy,
                    secondName: nameof(IngestionTracking.CreatedBy)),
                Parameter: nameof(IngestionTracking.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputIngestionTracking.UpdatedDate,
                    secondDate: storageIngestionTracking.UpdatedDate,
                    secondDateName: nameof(IngestionTracking.UpdatedDate)),
                Parameter: nameof(IngestionTracking.UpdatedDate)));
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            string first,
            string second) => new
            {
                Condition = first != second,
                Message = $"Expected value to be '{first}' but found '{second}'."
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

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date) => new
        {
            Condition = await IsDateNotRecentAsync(date),
            Message = "Date is not recent"
        };

        private async ValueTask<bool> IsDateNotRecentAsync(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidIngestionTrackingException = new InvalidIngestionTrackingException(
                message: "Invalid ingestion tracking. Please investigate.");

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