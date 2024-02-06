using System;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private void ValidateSubscriberAgreementOnAdd(SubscriberAgreement subscriberAgreement)
        {
            ValidateSubscriberAgreementIsNotNull(subscriberAgreement);

            Validate(
                (Rule: IsInvalid(subscriberAgreement.Id), Parameter: nameof(SubscriberAgreement.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(subscriberAgreement.CreatedDate), Parameter: nameof(SubscriberAgreement.CreatedDate)),
                (Rule: IsInvalid(subscriberAgreement.CreatedBy), Parameter: nameof(SubscriberAgreement.CreatedBy)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedDate), Parameter: nameof(SubscriberAgreement.UpdatedDate)),
                (Rule: IsInvalid(subscriberAgreement.UpdatedBy), Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: subscriberAgreement.UpdatedDate,
                    secondDate: subscriberAgreement.CreatedDate,
                    secondDateName: nameof(SubscriberAgreement.CreatedDate)),
                Parameter: nameof(SubscriberAgreement.UpdatedDate)),

                (Rule: IsNotSame(
                    first: subscriberAgreement.UpdatedBy,
                    second: subscriberAgreement.CreatedBy,
                    secondName: nameof(SubscriberAgreement.CreatedBy)),
                Parameter: nameof(SubscriberAgreement.UpdatedBy)),

                (Rule: IsNotRecent(subscriberAgreement.CreatedDate), Parameter: nameof(SubscriberAgreement.CreatedDate)));
        }

        public void ValidateSubscriberAgreementId(Guid subscriberAgreementId) =>
            Validate((Rule: IsInvalid(subscriberAgreementId), Parameter: nameof(SubscriberAgreement.Id)));

        private static void ValidateSubscriberAgreementIsNotNull(SubscriberAgreement subscriberAgreement)
        {
            if (subscriberAgreement is null)
            {
                throw new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
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
            var invalidSubscriberAgreementException = 
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSubscriberAgreementException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSubscriberAgreementException.ThrowIfContainsErrors();
        }
    }
}