// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;

namespace LHDS.Core.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingService
    {
        private static void ValidateTerminologyPollIsNotNull(TerminologyPoll terminologyPoll)
        {
            if (terminologyPoll is null)
            {
                throw new NullTerminologyPollProcessingException(
                    message: "Terminology poll is null.");
            }
        }

        public void ValidateTerminologyPollId(Guid terminologyPollId) =>
            Validate((Rule: IsInvalid(terminologyPollId), Parameter: nameof(TerminologyPoll.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        public void ValidateResourceType(string resourceType) =>
            Validate((Rule: IsInvalid(resourceType), Parameter: "resourceType"));

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTerminologyPollsProcessingException =
                new InvalidArgumentTerminologyPollsProcessingException(
                    message: "Invalid argument terminology poll processing. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTerminologyPollsProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTerminologyPollsProcessingException.ThrowIfContainsErrors();
        }
    }
}