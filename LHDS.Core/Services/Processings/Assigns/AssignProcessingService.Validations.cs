// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.Assigns.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Assigns
{
    public partial class AssignProcessingService
    {
        private void ValidateOnMatchAddress(string address)
        {
            Validate(
                createException: () => new InvalidArgumentAssignProcessingException(
                    message: "Invalid address. Please correct the errors and try again."),

                (Rule: IsInvalid(address), Parameter: nameof(address)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

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
