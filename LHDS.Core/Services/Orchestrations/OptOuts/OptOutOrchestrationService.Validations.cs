// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private static void ValidateOptOutFileIsNotNull(byte[] optOutFile)
        {
            Validate((Rule: IsInvalid(optOutFile), Parameter: "OptOutFile"));
        }
        private static void ValidateRequestIdIsNotNull(string requestId)
        {
            Validate((Rule: IsInvalid(requestId), Parameter: "RequestId"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(byte[] data) => new
        {
            Condition = data == null || data.Length == 0,
            Message = "Data is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentRetieveOptOutStatusOrchestrationException = new InvalidArgumentRetieveOptOutStatusOrchestrationException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentRetieveOptOutStatusOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentRetieveOptOutStatusOrchestrationException.ThrowIfContainsErrors();
        }
        private string GetValidationSummary(IDictionary data)
        {
            StringBuilder validationSummary = new StringBuilder();

            foreach (DictionaryEntry entry in data)
            {
                string errorSummary = ((List<string>)entry.Value)
                    .Select((string value) => value)
                    .Aggregate((string current, string next) => current + ", " + next);

                validationSummary.Append($"{entry.Key} => {errorSummary};  ");
            }

            return validationSummary.ToString();
        }

    }
}
