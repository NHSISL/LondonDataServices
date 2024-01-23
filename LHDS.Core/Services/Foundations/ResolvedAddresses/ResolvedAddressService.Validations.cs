using System;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService
    {
        private void ValidateResolvedAddressOnAdd(ResolvedAddress resolvedAddress)
        {
            ValidateResolvedAddressIsNotNull(resolvedAddress);

            Validate(
                (Rule: IsInvalid(resolvedAddress.Id), Parameter: nameof(ResolvedAddress.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)),
                (Rule: IsInvalid(resolvedAddress.CreatedBy), Parameter: nameof(ResolvedAddress.CreatedBy)),
                (Rule: IsInvalid(resolvedAddress.UpdatedDate), Parameter: nameof(ResolvedAddress.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddress.UpdatedBy), Parameter: nameof(ResolvedAddress.UpdatedBy)));
        }

        private static void ValidateResolvedAddressIsNotNull(ResolvedAddress resolvedAddress)
        {
            if (resolvedAddress is null)
            {
                throw new NullResolvedAddressException(message: "ResolvedAddress is null.");
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidResolvedAddressException = 
                new InvalidResolvedAddressException(
                    message: "Invalid resolvedAddress. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidResolvedAddressException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidResolvedAddressException.ThrowIfContainsErrors();
        }
    }
}