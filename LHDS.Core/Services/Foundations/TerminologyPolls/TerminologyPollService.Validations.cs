// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService
    {
        private async ValueTask ValidateTerminologyPollOnAddAsync(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);

            Validate(
                (Rule: IsInvalid(terminologyPoll.Id), Parameter: nameof(TerminologyPoll.Id)),
                (Rule: IsInvalid(terminologyPoll.ResourceType), Parameter: nameof(TerminologyPoll.ResourceType)),
                (Rule: IsInvalid(terminologyPoll.LastPoll), Parameter: nameof(TerminologyPoll.LastPoll)),
                (Rule: IsInvalid(terminologyPoll.CreatedDate), Parameter: nameof(TerminologyPoll.CreatedDate)),
                (Rule: IsInvalid(terminologyPoll.CreatedBy), Parameter: nameof(TerminologyPoll.CreatedBy)),
                (Rule: IsInvalid(terminologyPoll.UpdatedDate), Parameter: nameof(TerminologyPoll.UpdatedDate)),
                (Rule: IsInvalid(terminologyPoll.UpdatedBy), Parameter: nameof(TerminologyPoll.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: terminologyPoll.UpdatedDate,
                    secondDate: terminologyPoll.CreatedDate,
                    secondDateName: nameof(TerminologyPoll.CreatedDate)),
                Parameter: nameof(TerminologyPoll.UpdatedDate)),

                (Rule: IsNotSame(
                    first: terminologyPoll.UpdatedBy,
                    second: terminologyPoll.CreatedBy,
                    secondName: nameof(TerminologyPoll.CreatedBy)),
                Parameter: nameof(TerminologyPoll.UpdatedBy)),

                (Rule: await IsNotRecentAsync(terminologyPoll.CreatedDate), Parameter: nameof(TerminologyPoll.CreatedDate)));
        }

        private async ValueTask ValidateTerminologyPollOnModifyAsync(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);

            Validate(
                (Rule: IsInvalid(terminologyPoll.Id), Parameter: nameof(TerminologyPoll.Id)),
                (Rule: IsInvalid(terminologyPoll.ResourceType), Parameter: nameof(TerminologyPoll.ResourceType)),
                (Rule: IsInvalid(terminologyPoll.LastPoll), Parameter: nameof(TerminologyPoll.LastPoll)),
                (Rule: IsInvalid(terminologyPoll.CreatedDate), Parameter: nameof(TerminologyPoll.CreatedDate)),
                (Rule: IsInvalid(terminologyPoll.CreatedBy), Parameter: nameof(TerminologyPoll.CreatedBy)),
                (Rule: IsInvalid(terminologyPoll.UpdatedDate), Parameter: nameof(TerminologyPoll.UpdatedDate)),
                (Rule: IsInvalid(terminologyPoll.UpdatedBy), Parameter: nameof(TerminologyPoll.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: terminologyPoll.UpdatedDate,
                    secondDate: terminologyPoll.CreatedDate,
                    secondDateName: nameof(TerminologyPoll.CreatedDate)),
                Parameter: nameof(TerminologyPoll.UpdatedDate)),

                (Rule: await IsNotRecentAsync(terminologyPoll.UpdatedDate), Parameter: nameof(terminologyPoll.UpdatedDate)));
        }

        public void ValidateTerminologyPollId(Guid terminologyPollId) =>
            Validate((Rule: IsInvalid(terminologyPollId), Parameter: nameof(TerminologyPoll.Id)));

        private static void ValidateStorageTerminologyPoll(
            TerminologyPoll maybeTerminologyPoll,
            Guid terminologyPollId)
        {
            if (maybeTerminologyPoll is null)
            {
                throw new NotFoundTerminologyPollException(terminologyPollId);
            }
        }

        private static void ValidateTerminologyPollIsNotNull(TerminologyPoll terminologyPoll)
        {
            if (terminologyPoll is null)
            {
                throw new NullTerminologyPollException(message: "TerminologyPoll is null.");
            }
        }

        private static void ValidateAgainstStorageTerminologyPollOnModify(
            TerminologyPoll inputTerminologyPoll,
            TerminologyPoll storageTerminologyPoll)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputTerminologyPoll.CreatedDate,
                    secondDate: storageTerminologyPoll.CreatedDate,
                    secondDateName: nameof(TerminologyPoll.CreatedDate)),
                Parameter: nameof(TerminologyPoll.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputTerminologyPoll.CreatedBy,
                    second: storageTerminologyPoll.CreatedBy,
                    secondName: nameof(TerminologyPoll.CreatedBy)),
                Parameter: nameof(TerminologyPoll.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputTerminologyPoll.UpdatedDate,
                    secondDate: storageTerminologyPoll.UpdatedDate,
                    secondDateName: nameof(TerminologyPoll.UpdatedDate)),
                Parameter: nameof(TerminologyPoll.UpdatedDate)));
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
            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidTerminologyPollException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidTerminologyPollException.ThrowIfContainsErrors();
        }
    }
}