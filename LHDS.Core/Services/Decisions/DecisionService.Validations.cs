// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;
using Xeptions;

namespace LHDS.Core.Services.Decisions
{
    public partial class DecisionService
    {
        private void ValidateDecisions(List<Decision> maybeDecisions)
        {
            ValidateDecisionsIsNotNull(maybeDecisions);

            var validations = maybeDecisions
                .SelectMany(decision => new[]
                {
                    (Rule: IsNotNull(decision.DecisionType), Parameter: nameof(Decision.DecisionType)),
                    (Rule: IsNotNull(decision.Patient), Parameter: nameof(Decision.Patient))
                })
                .ToArray();

            Validate<InvalidDecisionsException>(
                message: "Invalid decisions. Please correct the errors and try again.",
                validations: validations
            );
        }

        private static void ValidateDecisionsIsNotNull(List<Decision> decision)
        {
            if (decision is null)
            {
                throw new NullDecisionsException(message: "Decisions is null.");
            }
        }

        private static dynamic IsNotNull(DecisionType decisionType) => new
        {
            Condition = decisionType is null,
            Message = "DecisionType is required"
        };

        private static dynamic IsNotNull(Patient patient) => new
        {
            Condition = patient is null,
            Message = "Patient is required"
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
