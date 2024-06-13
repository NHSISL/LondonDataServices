// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService
    {
        public void ValidateResourceTypes(string[] resourceTypes) =>
            Validate((Rule: IsInvalid(resourceTypes), Parameter: "resourceTypes"));

        public void ValidateResourceType(string resourceType) =>
            Validate((Rule: IsInvalid(resourceType), Parameter: "resourceType"));

        public void ValidateResourceURL(string resourceURL) =>
            Validate((Rule: IsInvalid(resourceURL), Parameter: "resourceURL"));

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(string[] text) => new
        {
            Condition = text == null || text.Length == 0,
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentTerminologyMetaDataOrchestrationException =
                new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                    "Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentTerminologyMetaDataOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentTerminologyMetaDataOrchestrationException.ThrowIfContainsErrors();
        }
    }
}