// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
                (Rule: IsInvalid(resolvedAddress.UniqueReference), Parameter: nameof(ResolvedAddress.UniqueReference)),

                (Rule: IsInvalid(resolvedAddress.UnstructuredPostalAddress),
                    Parameter: nameof(ResolvedAddress.UnstructuredPostalAddress)),

                (Rule: IsInvalid(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)),
                (Rule: IsInvalid(resolvedAddress.CreatedBy), Parameter: nameof(ResolvedAddress.CreatedBy)),
                (Rule: IsInvalid(resolvedAddress.UpdatedDate), Parameter: nameof(ResolvedAddress.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddress.UpdatedBy), Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: resolvedAddress.UpdatedDate,
                    secondDate: resolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)),

                (Rule: IsNotSame(
                    first: resolvedAddress.UpdatedBy,
                    second: resolvedAddress.CreatedBy,
                    secondName: nameof(ResolvedAddress.CreatedBy)),
                Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsNotRecent(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)));
        }

        private void ValidateOnBulkAddResolvedAddresses(List<ResolvedAddress> resolvedAddresses, string fileName)
        {
            Validate(
                (Rule: IsInvalid(resolvedAddresses), Parameter: nameof(resolvedAddresses)),
                (Rule: IsInvalid(fileName), Parameter: nameof(fileName)));
        }

        private void ValidateOnBulkModifyResolvedAddresses(List<ResolvedAddress> resolvedAddresses)
        {
            Validate(
                (Rule: IsInvalid(resolvedAddresses), Parameter: nameof(resolvedAddresses)));
        }

        private void ValidateResolvedAddressOnModify(ResolvedAddress resolvedAddress)
        {
            ValidateResolvedAddressIsNotNull(resolvedAddress);

            Validate(
                (Rule: IsInvalid(resolvedAddress.Id), Parameter: nameof(ResolvedAddress.Id)),
                (Rule: IsInvalid(resolvedAddress.UniqueReference), Parameter: nameof(ResolvedAddress.UniqueReference)),

                (Rule: IsInvalid(resolvedAddress.UnstructuredPostalAddress),
                    Parameter: nameof(ResolvedAddress.UnstructuredPostalAddress)),

                (Rule: IsInvalid(resolvedAddress.CreatedDate), Parameter: nameof(ResolvedAddress.CreatedDate)),
                (Rule: IsInvalid(resolvedAddress.CreatedBy), Parameter: nameof(ResolvedAddress.CreatedBy)),
                (Rule: IsInvalid(resolvedAddress.UpdatedDate), Parameter: nameof(ResolvedAddress.UpdatedDate)),
                (Rule: IsInvalid(resolvedAddress.UpdatedBy), Parameter: nameof(ResolvedAddress.UpdatedBy)),

                (Rule: IsSame(
                    firstDate: resolvedAddress.UpdatedDate,
                    secondDate: resolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)),

                (Rule: IsNotRecent(resolvedAddress.UpdatedDate), Parameter: nameof(resolvedAddress.UpdatedDate)));
        }

        public void ValidateResolvedAddressId(Guid resolvedAddressId) =>
            Validate((Rule: IsInvalid(resolvedAddressId), Parameter: nameof(ResolvedAddress.Id)));

        private static void ValidateStorageResolvedAddress(ResolvedAddress maybeResolvedAddress, Guid resolvedAddressId)
        {
            if (maybeResolvedAddress is null)
            {
                throw new NotFoundResolvedAddressException(resolvedAddressId);
            }
        }

        private static void ValidateResolvedAddressIsNotNull(ResolvedAddress resolvedAddress)
        {
            if (resolvedAddress is null)
            {
                throw new NullResolvedAddressException(message: "ResolvedAddress is null.");
            }
        }

        private static void ValidateAgainstStorageResolvedAddressOnModify(ResolvedAddress inputResolvedAddress, ResolvedAddress storageResolvedAddress)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputResolvedAddress.CreatedDate,
                    secondDate: storageResolvedAddress.CreatedDate,
                    secondDateName: nameof(ResolvedAddress.CreatedDate)),
                Parameter: nameof(ResolvedAddress.CreatedDate)),

                (Rule: IsNotSame(
                    first: inputResolvedAddress.CreatedBy,
                    second: storageResolvedAddress.CreatedBy,
                    secondName: nameof(ResolvedAddress.CreatedBy)),
                Parameter: nameof(ResolvedAddress.CreatedBy)),

                (Rule: IsSame(
                    firstDate: inputResolvedAddress.UpdatedDate,
                    secondDate: storageResolvedAddress.UpdatedDate,
                    secondDateName: nameof(ResolvedAddress.UpdatedDate)),
                Parameter: nameof(ResolvedAddress.UpdatedDate)));
        }

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

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(List<ResolvedAddress> resolvedAddresses) => new
        {
            Condition = resolvedAddresses == null,
            Message = "ResolvedAddresses is required"
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