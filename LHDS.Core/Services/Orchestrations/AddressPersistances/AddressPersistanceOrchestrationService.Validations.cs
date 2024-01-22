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
        private static void ValidateAddressListOrchestrationOnProcess(List<Address> addressList)
        {
            Validate(
                (Rule: IsInvalid(addressList), Parameter: "AddressList"));
        }

        private static dynamic IsInvalid(List<Address> addressList) => new
        {
            Condition = addressList == null,
            Message = "Address list is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAddressPersistanceOrchestrationException =
                new InvalidArgumentAddressPersistanceOrchestrationException(
                    message:
                    "Invalid address persistance orchestration argument, please correct the errors and try again.");

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
