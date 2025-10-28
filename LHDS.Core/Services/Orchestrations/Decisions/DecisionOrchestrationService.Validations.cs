// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationService
    {
        private void ValidateBlobContainersIsNotNull()
        {
            if (this.blobContainers is null)
            {
                throw new NullBlobContainersDecisionOrchestrationException(
                    message: "Null blob container decision orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateDecisionConfigurationIsNotNull()
        {
            if (this.decisionConfiguration is null)
            {
                throw new NullDecisionConfigurationDecisionOrchestrationException(
                    message: "Null decision configuration decision orchestration exception, " +
                        "please correct the errors and try again.");
            }
        }

        private void ValidateDocumentRequirements(string content)
        {
            Validate<InvalidArgumentDecisionOrchestrationException>(
                () => new InvalidArgumentDecisionOrchestrationException(
                    "Invalid Decision orchestration argument(s), please correct the errors and try again."),
                (Rule: IsInvalid(content), Parameter: "Content"));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        internal virtual void Validate<T>(
            Func<T> exceptionFactory,
            params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            T invalidDataException = exceptionFactory();

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
