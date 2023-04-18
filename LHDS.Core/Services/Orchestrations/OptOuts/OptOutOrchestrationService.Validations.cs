// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;

namespace LHDS.Core.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationService
    {
        private static void ValidateOptOutFileIsNotNull(byte[] optOutFile)
        {
            Validate((Rule: IsInvalid(optOutFile), Parameter: "OptOutFile"));
        }

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
    }
}
