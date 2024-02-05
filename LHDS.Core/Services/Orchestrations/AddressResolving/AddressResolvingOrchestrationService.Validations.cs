// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Orchestrations.AddressResolvings.Exceptions;

namespace LHDS.Core.Services.Orchestrations.AddressResolvings
{
    internal partial class AddressResolvingOrchestrationService
    {
        private static void ValidateNormalisedAddress(AddressNormalisation normalisedAddress)
        {
            Validate(
                (Rule: IsInvalid(normalisedAddress), Parameter: "NormalisedAddressList"));
        }

        private static dynamic IsInvalid(AddressNormalisation normalisedAddress) => new
        {
            Condition = normalisedAddress == null,
            Message = "Normalised address list is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAddressResolvingOrchestrationException =
                new InvalidArgumentAddressResolvingOrchestrationException(
                    message: "Invalid normalised address resolving orchestration argument, " +
                    "please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentAddressResolvingOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentAddressResolvingOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
