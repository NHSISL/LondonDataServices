// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Decisions
{
    public partial class DecisionService
    {
        private void ValidateDecisions(List<Decision> maybeDecisions)
        {
            if (!maybeDecisions.Any())
            {
                return;
            }

            var validations = maybeDecisions
                .SelectMany((decision, i) => new[]
                {
                    (Rule: IsInvalid(decision.DecisionTypeName),
                        Parameter: $"Decisions[{i}].{nameof(Decision.DecisionTypeName)} - Id: {decision.Id}"),

                    (Rule: IsInvalid(decision.PatientNhsNumber),
                        Parameter: $"Decisions[{i}].{nameof(Decision.PatientNhsNumber)} - Id: {decision.Id}")
                })
                .ToArray();

            Validate<InvalidDecisionsException>(
                () => new InvalidDecisionsException(
                    "Invalid decisions. Please correct the errors and try again."),
                validations
            );
        }

        private void ValidateDecisionsAdopted(List<Decision> decisionsAdopted)
        {
            if (decisionsAdopted is null || !decisionsAdopted.Any())
            {
                throw new InvalidDecisionsException("Decisions required.");
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
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
