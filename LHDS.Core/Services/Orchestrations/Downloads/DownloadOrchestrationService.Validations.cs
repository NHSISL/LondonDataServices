// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads.Exceptions;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;

namespace LHDS.Core.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationService
    {
        private static void ValidateFileName(string fileName)
        {
            Validate((Rule: IsInvalid(fileName), Parameter: "FileName"));
        }

        private static void ValidateStorageDownload(Document maybeDocument, string fileName)
        {
            if (maybeDocument is null)
            {
                throw new NotFoundDownloadOrchestrationException(
                    message: $"Couldn't find download with file name: {fileName}.");
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentDownloadOrchestrationException = new InvalidArgumentDownloadOrchestrationException(
                message: "Invalid download orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentDownloadOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentDownloadOrchestrationException.ThrowIfContainsErrors();
        }
    }
}