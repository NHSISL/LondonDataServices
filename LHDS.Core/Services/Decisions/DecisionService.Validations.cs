// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Decisions
{
    public partial class DecisionService
    {
        private void ValidateDecisions(List<Decision> maybeDecisions)
        {
            ValidateDecisionsIsNotNull(maybeDecisions);

            Validate<InvalidDecisionsException>(
                message: "Invalid decisions. Please correct the errors and try again",
                (Rule: IsNotNull(maybeDecisions, nameof(maybeDecisions)), Parameter: nameof(maybeDecisions)),
                (Rule: new
                {
                    Condition = maybeDecisions.Exists(decision => decision is null),
                    Message = "One or more decisions in the list are null."
                }, Parameter: "Decisions"),
                (Rule: maybeDecisions.Exists(decision =>
                        IsNotNull(decision.DecisionType, nameof(Decision.DecisionType)).Condition),
                    Parameter: "DecisionType"),
                (Rule: maybeDecisions.Exists(decision =>
                        IsNotNull(decision.Patient, nameof(Decision.Patient)).Condition),
                    Parameter: "Patient")
            );
        }

        private static void ValidateDecisionsIsNotNull(List<Decision> decision)
        {
            if (decision is null)
            {
                throw new NullDecisionsException(message: "Decisions is null.");
            }
        }

        private static dynamic IsNotNull(object value, string parameterName) => new
        {
            Condition = value is null,
            Message = $"{parameterName} is required"
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
