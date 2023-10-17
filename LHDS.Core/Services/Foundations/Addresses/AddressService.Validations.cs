using System;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService
    {
        private void ValidateAddressOnAdd(Address address)
        {
            ValidateAddressIsNotNull(address);

            Validate(
                (Rule: IsInvalid(address.Id), Parameter: nameof(Address.Id)),

                // TODO: Add any other required validation rules

                (Rule: IsInvalid(address.CreatedDate), Parameter: nameof(Address.CreatedDate)),
                (Rule: IsInvalid(address.CreatedBy), Parameter: nameof(Address.CreatedBy)),
                (Rule: IsInvalid(address.UpdatedDate), Parameter: nameof(Address.UpdatedDate)),
                (Rule: IsInvalid(address.UpdatedBy), Parameter: nameof(Address.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: address.UpdatedDate,
                    secondDate: address.CreatedDate,
                    secondDateName: nameof(Address.CreatedDate)),
                Parameter: nameof(Address.UpdatedDate)));
        }

        private static void ValidateAddressIsNotNull(Address address)
        {
            if (address is null)
            {
                throw new NullAddressException(message: "Address is null.");
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAddressException = 
                new InvalidAddressException(
                    message: "Invalid address. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAddressException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAddressException.ThrowIfContainsErrors();
        }
    }
}