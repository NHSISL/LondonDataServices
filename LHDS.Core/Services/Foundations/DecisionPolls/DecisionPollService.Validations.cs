// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;

namespace LHDS.Core.Services.Foundations.DecisionPolls
{
    public partial class DecisionPollService
    {
        private async ValueTask ValidateDecisionPollOnAddAsync(DecisionPoll decisionPoll)
        {
            ValidateDecisionPollIsNotNull(decisionPoll);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(decisionPoll.Id), Parameter: nameof(DecisionPoll.Id)),
                (Rule: IsInvalid(decisionPoll.LastPoll), Parameter: nameof(DecisionPoll.LastPoll)),
                (Rule: IsInvalid(decisionPoll.CreatedDate), Parameter: nameof(DecisionPoll.CreatedDate)),
                (Rule: IsInvalid(decisionPoll.CreatedBy), Parameter: nameof(DecisionPoll.CreatedBy)),
                (Rule: IsInvalid(decisionPoll.UpdatedDate), Parameter: nameof(DecisionPoll.UpdatedDate)),
                (Rule: IsInvalid(decisionPoll.UpdatedBy), Parameter: nameof(DecisionPoll.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: decisionPoll.CreatedBy),
                Parameter: nameof(DecisionPoll.CreatedBy)),

                (Rule: IsNotSame(
                    firstDate: decisionPoll.UpdatedDate,
                    secondDate: decisionPoll.CreatedDate,
                    secondDateName: nameof(DecisionPoll.CreatedDate)),
                Parameter: nameof(DecisionPoll.UpdatedDate)),

                (Rule: IsNotSame(
                    first: decisionPoll.UpdatedBy,
                    second: decisionPoll.CreatedBy,
                    secondName: nameof(DecisionPoll.CreatedBy)),
                Parameter: nameof(DecisionPoll.UpdatedBy)),

                (Rule: await IsNotRecentAsync(decisionPoll.CreatedDate), Parameter: nameof(DecisionPoll.CreatedDate)));
        }

        private async ValueTask ValidateDecisionPollOnModifyAsync(DecisionPoll decisionPoll)
        {
            ValidateDecisionPollIsNotNull(decisionPoll);
            EntraUser currentUser = await this.securityBroker.GetCurrentUserAsync();

            Validate(
                (Rule: IsInvalid(decisionPoll.Id), Parameter: nameof(DecisionPoll.Id)),
                (Rule: IsInvalid(decisionPoll.LastPoll), Parameter: nameof(DecisionPoll.LastPoll)),
                (Rule: IsInvalid(decisionPoll.CreatedDate), Parameter: nameof(DecisionPoll.CreatedDate)),
                (Rule: IsInvalid(decisionPoll.CreatedBy), Parameter: nameof(DecisionPoll.CreatedBy)),
                (Rule: IsInvalid(decisionPoll.UpdatedDate), Parameter: nameof(DecisionPoll.UpdatedDate)),
                (Rule: IsInvalid(decisionPoll.UpdatedBy), Parameter: nameof(DecisionPoll.UpdatedBy)),

                (Rule: IsNotSame(
                    first: currentUser.EntraUserId,
                    second: decisionPoll.UpdatedBy),
                Parameter: nameof(DecisionPoll.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: decisionPoll.UpdatedDate,
                    secondDate: decisionPoll.CreatedDate,
                    secondDateName: nameof(DecisionPoll.CreatedDate)),
                Parameter: nameof(DecisionPoll.UpdatedDate)),

                (Rule: await IsNotRecentAsync(decisionPoll.UpdatedDate), Parameter: nameof(decisionPoll.UpdatedDate)));
        }

        public void ValidateDecisionPollId(Guid decisionPollId) =>
            Validate((Rule: IsInvalid(decisionPollId), Parameter: nameof(DecisionPoll.Id)));

        private static void ValidateStorageDecisionPoll(
            DecisionPoll maybeDecisionPoll,
            Guid decisionPollId)
        {
            if (maybeDecisionPoll is null)
            {
                throw new NotFoundDecisionPollException(decisionPollId);
            }
        }

        private static void ValidateDecisionPollIsNotNull(DecisionPoll decisionPoll)
        {
            if (decisionPoll is null)
            {
                throw new NullDecisionPollException(message: "DecisionPoll is null.");
            }
        }

        private static void ValidateAgainstStorageDecisionPollOnModify(
            DecisionPoll inputDecisionPoll,
            DecisionPoll storageDecisionPoll)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputDecisionPoll.CreatedDate,
                    secondDate: storageDecisionPoll.CreatedDate,
                    secondDateName: nameof(DecisionPoll.CreatedDate)),
                Parameter: nameof(DecisionPoll.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputDecisionPoll.CreatedBy,
                    second: storageDecisionPoll.CreatedBy,
                    secondName: nameof(DecisionPoll.CreatedBy)),
                Parameter: nameof(DecisionPoll.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputDecisionPoll.UpdatedDate,
                    secondDate: storageDecisionPoll.UpdatedDate,
                    secondDateName: nameof(DecisionPoll.UpdatedDate)),
                Parameter: nameof(DecisionPoll.UpdatedDate)));
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
            string first,
            string second) => new
            {
                Condition = first != second,
                Message = $"Expected value to be '{first}' but found '{second}'."
            };

        private async ValueTask<dynamic> IsNotRecentAsync(DateTimeOffset date)
        {
            var (isNotRecent, startDate, endDate) = await IsDateNotRecentAsync(date);

            return new
            {
                Condition = isNotRecent,
                Message = $"Date is not recent. Expected a value between {startDate} and {endDate} but found {date}"
            };
        }

        private async ValueTask<(bool IsNotRecent, DateTimeOffset StartDate, DateTimeOffset EndDate)>
            IsDateNotRecentAsync(DateTimeOffset date)
        {
            int pastThreshold = 90;
            int futureThreshold = 0;
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            if (currentDateTime == default)
            {
                return (false, default, default);
            }

            DateTimeOffset startDate = currentDateTime.AddSeconds(-pastThreshold);
            DateTimeOffset endDate = currentDateTime.AddSeconds(futureThreshold);
            bool isNotRecent = date < startDate || date > endDate;

            return (isNotRecent, startDate, endDate);
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidDecisionPollException =
                new InvalidDecisionPollException(
                    message: "Invalid decisionPoll. Please correct the errors and try again.");


            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDecisionPollException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDecisionPollException.ThrowIfContainsErrors();
        }
    }
}
