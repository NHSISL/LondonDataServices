// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Orchestrations.Ingres.Exceptions;

namespace LHDS.Core.Services.Orchestrations.Ingress
{
    public partial class IngressOrchestrationService
    {
        private void ValidateOnCheckForBatchComplete(Guid ingestionTrackingId)
        {
            Validate((Rule: IsInvalid(ingestionTrackingId),
                Parameter: nameof(ingestionTrackingId)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentIngressOrchestrationException = new InvalidArgumentIngressOrchestrationException(
                message: "Invalid ingress orchestration argument(s), please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentIngressOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentIngressOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
