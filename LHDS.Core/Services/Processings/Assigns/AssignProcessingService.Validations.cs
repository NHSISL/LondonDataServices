// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Models.Processings.AssignAddresses.Exceptions;

namespace LHDS.Core.Services.Processings.Assigns
{
    public partial class AssignProcessingService
    {
        private void ValidateOnMatchAddress(string address)
        {
            Validate(
                (Rule: IsInvalid(address), Parameter: nameof(address)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAssignAddressProcessingException =
                new InvalidArgumentAssignAddressProcessingException(
                    message: "Invalid address. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentAssignAddressProcessingException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentAssignAddressProcessingException.ThrowIfContainsErrors();
        }
    }
}
