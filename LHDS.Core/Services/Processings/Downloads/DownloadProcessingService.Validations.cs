// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Processings.Downloads.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Downloads
{
    public partial class DownloadProcessingService
    {
        public void ValidateDownloadArgs(string fileName) =>
            Validate<InvalidArgumentDownloadProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                validations: (Rule: IsInvalid(fileName), Parameter: nameof(Document.FileName)));

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

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