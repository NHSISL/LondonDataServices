// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;

namespace LHDS.Core.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeService
    {
        private async ValueTask ValidateSubscriberPracticeOnAddAsync(SubscriberPractice subscriberPractice)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(subscriberPractice.Id), Parameter: nameof(SubscriberPractice.Id)),

                (Rule: IsInvalid(subscriberPractice.SubscriberAgreementId), 
                    Parameter: nameof(SubscriberPractice.SubscriberAgreementId)),

                (Rule: IsInvalid(subscriberPractice.Name),
                    Parameter: nameof(SubscriberPractice.Name)),

                (Rule: IsInvalid(subscriberPractice.PracticeCode),
                    Parameter: nameof(SubscriberPractice.PracticeCode)),

                (Rule: IsInvalid(subscriberPractice.CreatedDate), Parameter: nameof(SubscriberPractice.CreatedDate)),
                (Rule: IsInvalid(subscriberPractice.CreatedBy), Parameter: nameof(SubscriberPractice.CreatedBy)),
                (Rule: IsInvalid(subscriberPractice.UpdatedDate), Parameter: nameof(SubscriberPractice.UpdatedDate)),
                (Rule: IsInvalid(subscriberPractice.UpdatedBy), Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: IsInvalidLength(subscriberPractice.CreatedBy, 255),
                    Parameter: nameof(SubscriberPractice.CreatedBy)),

                (Rule: IsInvalidLength(subscriberPractice.UpdatedBy, 255),
                    Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: subscriberPractice.UpdatedDate,
                    secondDate: subscriberPractice.CreatedDate,
                    secondDateName: nameof(SubscriberPractice.CreatedDate)),
                Parameter: nameof(SubscriberPractice.UpdatedDate)),

                 (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: subscriberPractice.CreatedBy),
                Parameter: nameof(SubscriberPractice.CreatedBy)),

                (Rule: IsNotSame(
                    first: subscriberPractice.UpdatedBy,
                    second: subscriberPractice.CreatedBy,
                    secondName: nameof(SubscriberPractice.CreatedBy)),
                Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: await IsNotRecentAsync(subscriberPractice.CreatedDate), Parameter: nameof(SubscriberPractice.CreatedDate)));
        }

        private async ValueTask ValidateSubscriberPracticeOnModifyAsync(SubscriberPractice subscriberPractice)
        {
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(subscriberPractice.Id), Parameter: nameof(SubscriberPractice.Id)),

                (Rule: IsInvalid(subscriberPractice.SubscriberAgreementId),
                    Parameter: nameof(SubscriberPractice.SubscriberAgreementId)),

               (Rule: IsInvalid(subscriberPractice.Name),
                    Parameter: nameof(SubscriberPractice.Name)),

                (Rule: IsInvalid(subscriberPractice.PracticeCode),
                    Parameter: nameof(SubscriberPractice.PracticeCode)),

                (Rule: IsInvalid(subscriberPractice.CreatedDate), Parameter: nameof(SubscriberPractice.CreatedDate)),
                (Rule: IsInvalid(subscriberPractice.CreatedBy), Parameter: nameof(SubscriberPractice.CreatedBy)),
                (Rule: IsInvalid(subscriberPractice.UpdatedDate), Parameter: nameof(SubscriberPractice.UpdatedDate)),
                (Rule: IsInvalid(subscriberPractice.UpdatedBy), Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: IsInvalidLength(subscriberPractice.CreatedBy, 255),
                    Parameter: nameof(SubscriberPractice.CreatedBy)),

                (Rule: IsInvalidLength(subscriberPractice.UpdatedBy, 255),
                    Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: subscriberPractice.UpdatedBy),
                Parameter: nameof(SubscriberPractice.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: subscriberPractice.UpdatedDate,
                    secondDate: subscriberPractice.CreatedDate,
                    secondDateName: nameof(SubscriberPractice.CreatedDate)),
                Parameter: nameof(SubscriberPractice.UpdatedDate)),

                (Rule: await IsNotRecentAsync(subscriberPractice.UpdatedDate),
                    Parameter: nameof(subscriberPractice.UpdatedDate)));
        }

        public void ValidateSubscriberPracticeId(Guid subscriberPracticeId) =>
            Validate((Rule: IsInvalid(subscriberPracticeId), Parameter: nameof(SubscriberPractice.Id)));

        private static void ValidateStorageSubscriberPractice(
            SubscriberPractice maybeSubscriberPractice,
            Guid subscriberPracticeId)
        {
            if (maybeSubscriberPractice is null)
            {
                throw new NotFoundSubscriberPracticeException(subscriberPracticeId);
            }
        }

        private static void ValidateSubscriberPracticeIsNotNull(SubscriberPractice subscriberPractice)
        {
            if (subscriberPractice is null)
            {
                throw new NullSubscriberPracticeException(message: "SubscriberPractice is null.");
            }
        }

        private static void ValidateAgainstStorageSubscriberPracticeOnModify(
            SubscriberPractice inputSubscriberPractice,
            SubscriberPractice storageSubscriberPractice)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputSubscriberPractice.CreatedDate,
                    secondDate: storageSubscriberPractice.CreatedDate,
                    secondDateName: nameof(SubscriberPractice.CreatedDate)),
                Parameter: nameof(SubscriberPractice.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputSubscriberPractice.CreatedBy,
                    second: storageSubscriberPractice.CreatedBy,
                    secondName: nameof(SubscriberPractice.CreatedBy)),
                Parameter: nameof(SubscriberPractice.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputSubscriberPractice.UpdatedDate,
                    secondDate: storageSubscriberPractice.UpdatedDate,
                    secondDateName: nameof(SubscriberPractice.UpdatedDate)),
                Parameter: nameof(SubscriberPractice.UpdatedDate)));
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

        private static dynamic IsInvalidLength(string text, int length) => new
        {
            Condition = IsInvalidStringLength(text, length),
            Message = "Text exceeded length requirement"
        };

        private static bool IsInvalidStringLength(string text, int length) =>
            (text ?? string.Empty).Length > length;

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
            var invalidSubscriberPracticeException =
                new InvalidSubscriberPracticeException(
                    message: "Invalid subscriberPractice. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSubscriberPracticeException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSubscriberPracticeException.ThrowIfContainsErrors();
        }
    }
}