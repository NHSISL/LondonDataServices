// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Orchestrations.TerminologyMetadatas.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.TerminologyMetadata
{
    public partial class TerminologyMetadataOrchestrationService
    {
        public void ValidateResourceTypes(string[] resourceTypes)
        {
            Validate(
                createException: () => new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                        "Please correct the errors and try again."),

                (Rule: IsInvalid(resourceTypes), Parameter: "resourceTypes"));
        }

        public void ValidateResourceType(string resourceType)
        {
            Validate(
                createException: () => new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                        "Please correct the errors and try again."),

                (Rule: IsInvalid(resourceType), Parameter: "resourceType"));

        }

        public void ValidateResourceURL(string resourceURL)
        {
            Validate(
                createException: () => new InvalidArgumentTerminologyMetaDataOrchestrationException(
                    message: "Invalid argument terminology metadata orchestration. " +
                        "Please correct the errors and try again."),

                (Rule: IsInvalid(resourceURL), Parameter: "resourceURL"));
        }

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