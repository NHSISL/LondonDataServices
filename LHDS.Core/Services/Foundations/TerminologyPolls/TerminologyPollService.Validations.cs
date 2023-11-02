using System;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;

namespace LHDS.Core.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollService
    {
        private void ValidateTerminologyPollOnAdd(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);

            Validate(
                (Rule: IsInvalid(terminologyPoll.Id), Parameter: nameof(TerminologyPoll.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(terminologyPoll.CreatedDate), Parameter: nameof(TerminologyPoll.CreatedDate)),
                (Rule: IsInvalid(terminologyPoll.CreatedBy), Parameter: nameof(TerminologyPoll.CreatedBy)),
                (Rule: IsInvalid(terminologyPoll.UpdatedDate), Parameter: nameof(TerminologyPoll.UpdatedDate)),
                (Rule: IsInvalid(terminologyPoll.UpdatedBy), Parameter: nameof(TerminologyPoll.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: terminologyPoll.UpdatedDate,
                    secondDate: terminologyPoll.CreatedDate,
                    secondDateName: nameof(TerminologyPoll.CreatedDate)),
                Parameter: nameof(TerminologyPoll.UpdatedDate)));
        }

        private static void ValidateTerminologyPollIsNotNull(TerminologyPoll terminologyPoll)
        {
            if (terminologyPoll is null)
            {
                throw new NullTerminologyPollException(message: "TerminologyPoll is null.");
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