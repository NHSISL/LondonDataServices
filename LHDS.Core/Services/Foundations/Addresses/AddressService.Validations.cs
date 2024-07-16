// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
                (Rule: IsInvalid(address.CreatedDate), Parameter: nameof(Address.CreatedDate)),
                (Rule: IsInvalid(address.CreatedBy), Parameter: nameof(Address.CreatedBy)),
                (Rule: IsInvalid(address.UpdatedDate), Parameter: nameof(Address.UpdatedDate)),
                (Rule: IsInvalid(address.UpdatedBy), Parameter: nameof(Address.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: address.UpdatedDate,
                    secondDate: address.CreatedDate,
                    secondDateName: nameof(Address.CreatedDate)),
                Parameter: nameof(Address.UpdatedDate)),

                (Rule: IsNotSame(
                    first: address.UpdatedBy,
                    second: address.CreatedBy,
                    secondName: nameof(Address.CreatedBy)),
                Parameter: nameof(Address.UpdatedBy)),

                (Rule: IsNotRecent(address.CreatedDate), Parameter: nameof(Address.CreatedDate)));
        }

        private void ValidateOnBulkAddAddresses(List<Address> addresses, string fileName)
        {
            Validate(
                (Rule: IsInvalid(addresses), Parameter: nameof(addresses)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateAddressOnModify(Address address)
        {
            ValidateAddressIsNotNull(address);

            Validate(
                (Rule: IsInvalid(address.Id), Parameter: nameof(Address.Id)),
                (Rule: IsInvalid(address.CreatedDate), Parameter: nameof(Address.CreatedDate)),
                (Rule: IsInvalid(address.CreatedBy), Parameter: nameof(Address.CreatedBy)),
                (Rule: IsInvalid(address.UpdatedDate), Parameter: nameof(Address.UpdatedDate)),
                (Rule: IsInvalid(address.UpdatedBy), Parameter: nameof(Address.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: address.UpdatedDate,
                    secondDate: address.CreatedDate,
                    secondDateName: nameof(Address.CreatedDate)),
                Parameter: nameof(Address.UpdatedDate)),

                (Rule: IsNotRecent(address.UpdatedDate), Parameter: nameof(address.UpdatedDate)));
        }

        public void ValidateAddressId(Guid addressId) =>
            Validate((Rule: IsInvalid(addressId), Parameter: nameof(Address.Id)));

        public void ValidatePostCode(string postCode) =>
            Validate((Rule: IsInvalid(postCode), Parameter: "postCode"));

        private static void ValidateStorageAddress(Address maybeAddress, Guid addressId)
        {
            if (maybeAddress is null)
            {
                throw new NotFoundAddressException(addressId);
            }
        }

        private static void ValidateAddressIsNotNull(Address address)
        {
            if (address is null)
            {
                throw new NullAddressException(message: "Address is null.");
            }
        }

        private static void ValidateAgainstStorageAddressOnModify(Address inputAddress, Address storageAddress)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAddress.CreatedDate,
                    secondDate: storageAddress.CreatedDate,
                    secondDateName: nameof(Address.CreatedDate)),
                Parameter: nameof(Address.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputAddress.CreatedBy,
                    second: storageAddress.CreatedBy,
                    secondName: nameof(Address.CreatedBy)),
                Parameter: nameof(Address.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputAddress.UpdatedDate,
                    secondDate: storageAddress.UpdatedDate,
                    secondDateName: nameof(Address.UpdatedDate)),
                Parameter: nameof(Address.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(List<Address> addresses) => new
        {
            Condition = addresses == null,
            Message = "Addresses is required"
        };

        private static dynamic IsInvalid(string? text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
           string first,
           string second,
           string secondName) => new
           {
               Condition = first != second,
               Message = $"Text is not the same as {secondName}"
           };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

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