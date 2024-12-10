// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.ConfigImportExportTool.Models.Foundations.CsvHelpers.Exceptions;
using Xeptions;

namespace LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers
{
    internal partial class CsvHelperService
    {
        private void ValidateMapCsvToObjectArguments(string data)
        {
            Validate<InvalidArgumentCsvHelperException>(
                message: "Invalid csv helper argument(s), please correct the errors and try again.",
                (Rule: IsInvalid(data), Parameter: nameof(data)));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateObjectListIsNotNull<T>(List<T> objectList)
        {
            if (objectList is null)
            {
                throw new InvalidArgumentCsvHelperException(
                    message: "Invalid csv helper argument(s), please correct the errors and try again.");
            }
        }

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T)Activator.CreateInstance(typeof(T), message);

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
