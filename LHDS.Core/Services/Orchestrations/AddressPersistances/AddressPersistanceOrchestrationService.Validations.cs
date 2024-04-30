// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;

namespace LHDS.Core.Services.Orchestrations.AddressPersistances
{
    internal partial class AddressPersistanceOrchestrationService
    {
        virtual internal void ValidateAddressPersistenceOrchestration(List<Address> addressList, string fileName)
        {
            Validate(
                (Rule: IsInvalid(addressList), Parameter: "addressList"),
                (Rule: IsInvalid(fileName), Parameter: "fileName"));
        }

        private static dynamic IsInvalid(List<Address> addressList) => new
        {
            Condition = addressList == null,
            Message = "Address list is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistenceOrchestrationException(
                    message:
                    "Invalid address persistence orchestration argument, please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentAddressPersistanceOrchestrationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentAddressPersistanceOrchestrationException.ThrowIfContainsErrors();
        }
    }
}
