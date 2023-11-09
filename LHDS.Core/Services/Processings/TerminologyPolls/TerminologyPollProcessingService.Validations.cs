// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingService
    {
        private void ValidateTerminologyPollOnAdd(TerminologyPoll terminologyPoll)
        {
            ValidateTerminologyPollIsNotNull(terminologyPoll);
        }

        private static void ValidateTerminologyPollIsNotNull(TerminologyPoll terminologyPoll)
        {
            if (terminologyPoll is null)
            {
                throw new NullTerminologyPollException(message: "Terminology poll is null.");
            }
        }

        public void ValidateTerminologyPollId(Guid terminologyPollId) =>
            Validate((Rule: IsInvalid(terminologyPollId), Parameter: nameof(TerminologyPoll.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

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