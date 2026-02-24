// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.FileNameValidations.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.FileNameValidations
{
    public partial class FileNameValidationService
    {
        private static void ValidateArguments(string fileName)
        {
            Validate<InvalidArgumentFileNameValidationServiceException>(
                message: "Invalid file name validation argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate<T>(
            string message,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException?.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException?.ThrowIfContainsErrors();
        }
    }
}