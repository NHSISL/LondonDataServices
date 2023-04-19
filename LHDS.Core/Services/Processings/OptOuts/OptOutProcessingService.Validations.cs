// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.OptOuts;
using LHDS.Core.Models.Processings.OptOuts.Exceptions;

namespace LHDS.Core.Services.Processings.OptOuts
{
    public partial class OptOutProcessingService
    {
        private static void ValidateOptOutProcessingOnRetrieveOrAdd(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingOnModify(OptOut optOut)
        {
            ValidateOptOutProcessingIsNotNull(optOut);
        }

        private static void ValidateOptOutProcessingIsNotNull(OptOut optOut)
        {
            if (optOut is null)
            {
                throw new NullOptOutProcessingException();
            }
        }

        public void ValidateOptOutId(Guid optOutId) =>
            Validate((Rule: IsInvalid(optOutId), Parameter: nameof(OptOut.Id)));

        public void ValidateOptOutNhsNumber(string optOutNhsNumber) =>
            Validate((Rule: IsInvalid(optOutNhsNumber), Parameter: nameof(OptOut.NhsNumber)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentOptOutProcessingException = new InvalidArgumentOptOutProcessingException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentOptOutProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentOptOutProcessingException.ThrowIfContainsErrors();
        }
    }
}

