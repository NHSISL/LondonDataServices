// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Tpp.Exceptions;
using LHDS.Core.Models.Orchestrations.Tpp.Exceptions;

namespace LHDS.Core.Services.Orchestrations.Tpp
{
    public partial class TppOrchestrationService
    {
        private static void ValidateDocumentIsNotNull(Document document)
        {
            if (document is null)
            {
                throw new NullTppDocumentException(message: "Document is Null");
            }
        }

        private static void ValidateDocumentFileNameIsNotNull(string fileName) =>
            Validate((Rule: IsInvalid(fileName), Parameter: "FileName"));

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentException = new InvalidArgumentException(
                message: "Invalid TPP orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentException.ThrowIfContainsErrors();
        }
    }
}