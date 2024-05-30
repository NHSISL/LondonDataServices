// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Processings.Addresses
{
    public partial class AddressProcessingService : IAddressProcessingService
    {
        private void ValidateAddress(Address address)
        {
            ValidateAddressIsNotNull(address);
        }

        private void ValidateAddresses(List<Address> addresses)
        {
            ValidateAddressesIsNotNull(addresses);
        }

        private void ValidateAddress(string address)
        {
            Validate<InvalidArgumentAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(address), Parameter: "Address"));
        }

        private static void ValidateAddressIsNotNull(Address address)
        {
            if (address is null)
            {
                throw new NullAddressProcessingException(message: "Address is null.");
            }
        }

        private static void ValidateAddressesIsNotNull(List<Address> addresses)
        {
            if (addresses is null)
            {
                throw new NullAddressProcessingException(message: "Addresses is null.");
            }
        }

        public void ValidateAddressId(Guid addressId) =>
            Validate<InvalidArgumentAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(addressId), Parameter: nameof(Address.Id)));

        public void ValidatePostCode(string postCode) =>
            Validate<InvalidArgumentAddressProcessingException>(
                message: "Invalid argument(s). Please correct the errors and try again.",
                (Rule: IsInvalid(postCode), Parameter: "postCode"));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate<T>(string message, params (dynamic Rule, string Parameter)[] validations)
            where T : Xeption
        {
            var invalidDataException = (T?)Activator.CreateInstance(typeof(T), message);

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidDataException?.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidDataException?.ThrowIfContainsErrors();
        }
    }
}
