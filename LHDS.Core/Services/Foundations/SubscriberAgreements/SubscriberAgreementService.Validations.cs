// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private async ValueTask ValidateSubscriberAgreementOnAddAsync(SubscriberAgreement subscriberAgreement)
        {
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                createException: () => new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again."),

                (Rule: IsInvalid(subscriberAgreement.Id), Parameter: nameof(SubscriberAgreement.Id)),
                (Rule: IsInvalid(subscriberAgreement.SupplierId), Parameter: nameof(SubscriberAgreement.SupplierId)),

                (Rule: IsInvalid(subscriberAgreement.SupplierSharingAgreementShortName),
                    Parameter: nameof(SubscriberAgreement.SupplierSharingAgreementShortName)),

                (Rule: IsInvalid(subscriberAgreement.CreatedDate), Parameter: nameof(SubscriberAgreement.CreatedDate)),
                (Rule: IsInvalid(subscriberAgreement.CreatedBy), Parameter: nameof(SubscriberAgreement.CreatedBy)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedDate), Parameter: nameof(SubscriberAgreement.UpdatedDate)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedBy), Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsInvalidLength(subscriberAgreement.SupplierSharingAgreementShortName, 128),
                    Parameter: nameof(SubscriberAgreement.SupplierSharingAgreementShortName)),

                (Rule: IsInvalidLength(subscriberAgreement.CreatedBy, 255),
                    Parameter: nameof(SubscriberAgreement.CreatedBy)),

                (Rule: IsInvalidLength(subscriberAgreement.UpdatedBy, 255),
                    Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: subscriberAgreement.UpdatedDate,
                    secondDate: subscriberAgreement.CreatedDate,
                    secondDateName: nameof(SubscriberAgreement.CreatedDate)),
                Parameter: nameof(SubscriberAgreement.UpdatedDate)),

                 (Rule: IsNotSame(
                    first: currentUserId,
                    second: subscriberAgreement.CreatedBy),
                Parameter: nameof(SubscriberAgreement.CreatedBy)),

                (Rule: IsNotSame(
                    first: subscriberAgreement.UpdatedBy,
                    second: subscriberAgreement.CreatedBy,
                    secondName: nameof(SubscriberAgreement.CreatedBy)),
                Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: await IsNotRecentAsync(subscriberAgreement.CreatedDate),
                Parameter: nameof(SubscriberAgreement.CreatedDate)));
        }

        private async ValueTask ValidateSubscriberAgreementOnModifyAsync(SubscriberAgreement subscriberAgreement)
        {
            string currentUserId = await this.securityAuditBroker.GetUserIdAsync();

            Validate(
                createException: () => new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again."),

                (Rule: IsInvalid(subscriberAgreement.Id), Parameter: nameof(SubscriberAgreement.Id)),
                (Rule: IsInvalid(subscriberAgreement.SupplierId), Parameter: nameof(SubscriberAgreement.SupplierId)),

                (Rule: IsInvalid(subscriberAgreement.SupplierSharingAgreementShortName),
                    Parameter: nameof(SubscriberAgreement.SupplierSharingAgreementShortName)),

                (Rule: IsInvalid(subscriberAgreement.CreatedDate), Parameter: nameof(SubscriberAgreement.CreatedDate)),
                (Rule: IsInvalid(subscriberAgreement.CreatedBy), Parameter: nameof(SubscriberAgreement.CreatedBy)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedDate), Parameter: nameof(SubscriberAgreement.UpdatedDate)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedBy), Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsInvalidLength(subscriberAgreement.SupplierSharingAgreementShortName, 128),
                    Parameter: nameof(SubscriberAgreement.SupplierSharingAgreementShortName)),

                (Rule: IsInvalidLength(subscriberAgreement.CreatedBy, 255),
                    Parameter: nameof(SubscriberAgreement.CreatedBy)),

                (Rule: IsInvalidLength(subscriberAgreement.UpdatedBy, 255),
                    Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUserId,
                    second: subscriberAgreement.UpdatedBy),
                Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: subscriberAgreement.UpdatedDate,
                    secondDate: subscriberAgreement.CreatedDate,
                    secondDateName: nameof(SubscriberAgreement.CreatedDate)),
                Parameter: nameof(SubscriberAgreement.UpdatedDate)),

                (Rule: await IsNotRecentAsync(subscriberAgreement.UpdatedDate),
                    Parameter: nameof(subscriberAgreement.UpdatedDate)));
        }

        public void ValidateSubscriberAgreementId(Guid subscriberAgreementId)
        {
            Validate(
                createException: () => new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again."), 
                
                (Rule: IsInvalid(subscriberAgreementId), Parameter: nameof(SubscriberAgreement.Id)));
        }

        private static void ValidateStorageSubscriberAgreement(
            SubscriberAgreement maybeSubscriberAgreement,
            Guid subscriberAgreementId)
        {
            if (maybeSubscriberAgreement is null)
            {
                throw new NotFoundSubscriberAgreementException(subscriberAgreementId);
            }
        }

        private static void ValidateSubscriberAgreementIsNotNull(SubscriberAgreement subscriberAgreement)
        {
            if (subscriberAgreement is null)
            {
                throw new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");
            }
        }

        private static void ValidateAgainstStorageSubscriberAgreementOnModify(
            SubscriberAgreement inputSubscriberAgreement,
            SubscriberAgreement storageSubscriberAgreement)
        {
            Validate(
                createException: () => new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again."),

                (Rule: IsNotSame(
                    firstDate: inputSubscriberAgreement.CreatedDate,
                    secondDate: storageSubscriberAgreement.CreatedDate,
                    secondDateName: nameof(SubscriberAgreement.CreatedDate)),
                Parameter: nameof(SubscriberAgreement.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputSubscriberAgreement.CreatedBy,
                    second: storageSubscriberAgreement.CreatedBy,
                    secondName: nameof(SubscriberAgreement.CreatedBy)),
                Parameter: nameof(SubscriberAgreement.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputSubscriberAgreement.UpdatedDate,
                    secondDate: storageSubscriberAgreement.UpdatedDate,
                    secondDateName: nameof(SubscriberAgreement.UpdatedDate)),
                Parameter: nameof(SubscriberAgreement.UpdatedDate)));
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

        private static void Validate<T>(
            Func<T> createException,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException.ThrowIfContainsErrors();
        }
    }
}