// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Models.Foundations.Assigns.Exceptions;

namespace LHDS.Core.Services.Foundations.Assigns
{
    public partial class AssignService
    {
        private void ValidateOnMatchAddress(string address)
        {
            Validate(
                (Rule: IsInvalid(address), Parameter: nameof(address)));
        }

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentAssignException =
                new InvalidArgumentAssignException(
                    message: "Invalid address. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentAssignException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentAssignException.ThrowIfContainsErrors();
        }
    }
}
